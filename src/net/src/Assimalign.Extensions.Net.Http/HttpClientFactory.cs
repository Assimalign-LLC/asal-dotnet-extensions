using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Assimalign.Extensions.Net.Http;

using Assimalign.Extensions.Net.Http.Internal;


public sealed class HttpClientFactory : IHttpClientFactory, IHttpMessageHandlerFactory
{
    private static readonly TimerCallback _cleanupCallback = (s) => ((HttpClientFactory)s).CleanupTimerTick();

    // Default time of 10s for cleanup seems reasonable.
    // Quick math:
    // 10 distinct named clients * expiry time >= 1s = approximate cleanup queue of 100 items
    //
    // This seems frequent enough. We also rely on GC occurring to actually trigger disposal.
    private readonly TimeSpan DefaultCleanupInterval = TimeSpan.FromSeconds(10);

    /* We use a new timer for each regular cleanup cycle, protected with a lock. Note that this scheme
     * doesn't give us anything to dispose, as the timer is started/stopped as needed.
     *
     * There's no need for the factory itself to be disposable. If you stop using it, eventually everything will
     * get reclaimed.
     */
    private Timer clientFactoryCleanupTimer;
    private readonly object clientFactoryCleanupTimerLock;
    private readonly object clientFactoryCleanupActiveLock;
    private readonly TimerCallback clientFactoryExpiryCallback;
    private readonly HttpClientFactoryOptions clientFactoryOptions;

    /* Collection of 'active' handlers.
     *
     * Using lazy for synchronization to ensure that only one instance of HttpMessageHandler is created
     * for each name.
     * internal for tests
    */
    private readonly ConcurrentDictionary<string, Lazy<HttpMessageHandlerActiveTrackingEntry>> clientFactoryActiveHandlers;

    /* Collection of 'expired' but not yet disposed handlers.
     *
     * Used when we're rotating handlers so that we can dispose HttpMessageHandler instances once they
     * are eligible for garbage collection.
     *
     * internal for tests
    */
    private readonly ConcurrentQueue<HttpMessageHandlerExpiredTrackingEntry> clientFactoryExpiredHandlers;


    private readonly Func<string, Lazy<HttpMessageHandlerActiveTrackingEntry>> clientFactoryTrackingEntry;

    private HttpClientFactory()
    {
        this.clientFactoryOptions ??= new HttpClientFactoryOptions();
        this.clientFactoryActiveHandlers = new ConcurrentDictionary<string, Lazy<HttpMessageHandlerActiveTrackingEntry>>(StringComparer.Ordinal);
        this.clientFactoryTrackingEntry = (name) =>
        {
            return new Lazy<HttpMessageHandlerActiveTrackingEntry>(() => CreateHandlerEntry(name), LazyThreadSafetyMode.ExecutionAndPublication);
        };
        this.clientFactoryExpiredHandlers = new ConcurrentQueue<HttpMessageHandlerExpiredTrackingEntry>();
        this.clientFactoryExpiryCallback = ExpiryTimerTick;
        this.clientFactoryCleanupTimerLock = new object();
        this.clientFactoryCleanupActiveLock = new object();
    }
    private HttpClientFactory(HttpClientFactoryOptions options) : this()
    {
        this.clientFactoryOptions = options;
    }

    public HttpClient CreateClient(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        var handler = CreateHandler(name);
        var client = new HttpClient(handler, disposeHandler: false);

        foreach (var action in clientFactoryOptions.HttpClientActions)
        {
            action.Invoke(client);
        }

        return client;
    }

    public HttpMessageHandler CreateHandler(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        var entry = clientFactoryActiveHandlers.GetOrAdd(name, clientFactoryTrackingEntry).Value;

        StartHandlerEntryTimer(entry);

        return entry.Handler;
    }
    public static IHttpClientFactory CreateHttpClientFactory() => new HttpClientFactory();
    public static IHttpClientFactory CreateHttpClientFactory(Action<HttpClientFactoryOptions> configure)
    {
        var options = new HttpClientFactoryOptions();

        configure.Invoke(options);

        return new HttpClientFactory(options);
    }
    public static IHttpMessageHandlerFactory CreateHttpMessageHandlerFactory(Action<HttpMessageHandlerBuilder> configure)
    {
        var options = new HttpClientFactoryOptions();

        options.AddHttpHandlerBuilderAction(configure);

        return new HttpClientFactory(options);
    }

    private HttpMessageHandlerActiveTrackingEntry CreateHandlerEntry(string name)
    {
        try
        {
            var builder = new HttpMessageHandlerBuilderDefault()
            {
                Name = name
            };

            Action<HttpMessageHandlerBuilder> configure = Configure;

            configure.Invoke(builder);

            // Wrap the handler so we can ensure the inner handler outlives the outer handler.
            var handler = new HttpMessageHandlerLifetimeTracking(builder.Build());

            // Note that we can't start the timer here. That would introduce a very very subtle race condition
            // with very short expiry times. We need to wait until we've actually handed out the handler once
            // to start the timer.
            //
            // Otherwise it would be possible that we start the timer here, immediately expire it (very short
            // timer) and then dispose it without ever creating a client. That would be bad. It's unlikely
            // this would happen, but we want to be sure.
            return new HttpMessageHandlerActiveTrackingEntry(name, handler, clientFactoryOptions.HandlerLifetime);

            void Configure(HttpMessageHandlerBuilder builder)
            {
                foreach (var action in clientFactoryOptions.HttpMessageHandlerBuilderActions)
                {
                    action.Invoke(builder);
                }
            }
        }
        catch
        {
            throw;
        }
    }
    private void ExpiryTimerTick(object state)
    {
        var active = (HttpMessageHandlerActiveTrackingEntry)state;

        // The timer callback should be the only one removing from the active collection. If we can't find
        // our entry in the collection, then this is a bug.
        bool removed = clientFactoryActiveHandlers.TryRemove(active.Name, out Lazy<HttpMessageHandlerActiveTrackingEntry> found);
        Debug.Assert(removed, "Entry not found. We should always be able to remove the entry");
        Debug.Assert(object.ReferenceEquals(active, found.Value), "Different entry found. The entry should not have been replaced");

        /*  
         *   At this point the handler is no longer 'active' and will not be handed out to any new clients.
         *   However we haven't dropped our strong reference to the handler, so we can't yet determine if
         *   there are still any other outstanding references (we know there is at least one).
         *  
         *   We use a different state object to track expired handlers. This allows any other thread that acquired
         *   the 'active' entry to use it without safety problems.
         */
        var expired = new HttpMessageHandlerExpiredTrackingEntry(active);
        clientFactoryExpiredHandlers.Enqueue(expired);
        StartCleanupTimer();
    }
    private void StartHandlerEntryTimer(HttpMessageHandlerActiveTrackingEntry entry)
    {
        entry.StartExpiryTimer(clientFactoryExpiryCallback);
    }
    private void StartCleanupTimer()
    {
        lock (clientFactoryCleanupTimerLock)
        {
            if (clientFactoryCleanupTimer == null)
            {
                clientFactoryCleanupTimer = NonCapturingTimer.Create(_cleanupCallback, this, DefaultCleanupInterval, Timeout.InfiniteTimeSpan);
            }
        }
    }
    private void StopCleanupTimer()
    {
        lock (clientFactoryCleanupTimerLock)
        {
            clientFactoryCleanupTimer.Dispose();
            clientFactoryCleanupTimer = null;
        }
    }
    private void CleanupTimerTick()
    {
        // Stop any pending timers, we'll restart the timer if there's anything left to process after cleanup.
        //
        // With the scheme we're using it's possible we could end up with some redundant cleanup operations.
        // This is expected and fine.
        //
        // An alternative would be to take a lock during the whole cleanup process. This isn't ideal because it
        // would result in threads executing ExpiryTimer_Tick as they would need to block on cleanup to figure out
        // whether we need to start the timer.
        StopCleanupTimer();

        if (!Monitor.TryEnter(clientFactoryCleanupActiveLock))
        {
            // We don't want to run a concurrent cleanup cycle. This can happen if the cleanup cycle takes
            // a long time for some reason. Since we're running user code inside Dispose, it's definitely
            // possible.
            //
            // If we end up in that position, just make sure the timer gets started again. It should be cheap
            // to run a 'no-op' cleanup.
            StartCleanupTimer();
            return;
        }

        try
        {
            int initialCount = clientFactoryExpiredHandlers.Count;

            int disposedCount = 0;
            for (int i = 0; i < initialCount; i++)
            {
                // Since we're the only one removing from _expired, TryDequeue must always succeed.
                clientFactoryExpiredHandlers.TryDequeue(out HttpMessageHandlerExpiredTrackingEntry entry);
                Debug.Assert(entry != null, "Entry was null, we should always get an entry back from TryDequeue");

                if (entry.CanDispose)
                {
                    try
                    {
                        entry.InnerHandler.Dispose();
                        //entry.Scope?.Dispose();
                        disposedCount++;
                    }
                    catch (Exception ex)
                    {
                        //  Log.CleanupItemFailed(_logger, entry.Name, ex);
                    }
                }
                else
                {
                    // If the entry is still live, put it back in the queue so we can process it
                    // during the next cleanup cycle.
                    clientFactoryExpiredHandlers.Enqueue(entry);
                }
            }
        }
        finally
        {
            Monitor.Exit(clientFactoryCleanupActiveLock);
        }

        // We didn't totally empty the cleanup queue, try again later.
        if (!clientFactoryExpiredHandlers.IsEmpty)
        {
            StartCleanupTimer();
        }
    }
}