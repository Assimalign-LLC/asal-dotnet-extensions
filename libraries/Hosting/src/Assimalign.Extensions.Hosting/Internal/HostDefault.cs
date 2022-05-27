using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assimalign.Extensions.Hosting.Internal
{
    using Assimalign.Extensions.Hosting;
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.FileProviders;

    /// <summary>
    /// The default IHost implementation for IHostBackgroundService's
    /// </summary>
    internal sealed class HostDefault : IHost, IAsyncDisposable
    {
        private readonly ILogger<HostDefault> logger;
        private readonly IHostLifetime hostLifetime;
        private readonly HostApplicationLifetime applicationLifetime;
        private readonly HostOptions options;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IFileProvider fileProvider;
        private IEnumerable<IHostService> hostedServices;
        private volatile bool stopCalled;


        public HostDefault(
            IServiceProvider services,
            IHostEnvironment hostEnvironment,
            IFileProvider fileProvider,
            IHostApplicationLifetime applicationLifetime,
            ILogger<HostDefault> logger,
            IHostLifetime hostLifetime,
            IOptions<HostOptions> options)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            this.applicationLifetime = (applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime))) as HostApplicationLifetime;
            this.hostEnvironment = hostEnvironment;
            this.fileProvider = fileProvider;

            if (this.applicationLifetime is null)
            {
                throw new ArgumentException("Replacing IHostApplicationLifetime is not supported.", nameof(applicationLifetime));
            }
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IServiceProvider Services { get; }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            logger.Starting();

            using (var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, applicationLifetime.ApplicationStopping))
            {
                var combinedCancellationToken = combinedCancellationTokenSource.Token;

                await hostLifetime.WaitForStartAsync(combinedCancellationToken).ConfigureAwait(false);

                combinedCancellationToken.ThrowIfCancellationRequested();
                hostedServices = Services.GetService<IEnumerable<IHostService>>();

                foreach (IHostService hostedService in hostedServices)
                {
                    // Fire IHostedService.Start
                    await hostedService.StartAsync(combinedCancellationToken).ConfigureAwait(false);

                    if (hostedService is HostBackgroundService backgroundService)
                    {
                        _ = TryExecuteBackgroundServiceAsync(backgroundService);
                    }
                }

                // Fire IHostApplicationLifetime.Started
                applicationLifetime.NotifyStarted();
            }

            logger.Started();
        }

        private async Task TryExecuteBackgroundServiceAsync(HostBackgroundService backgroundService)
        {
            // backgroundService.ExecuteTask may not be set (e.g. if the derived class doesn't call base.StartAsync)
            Task backgroundTask = backgroundService.ExecuteTask;

            if (backgroundTask == null)
            {
                return;
            }

            try
            {
                await backgroundTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // When the host is being stopped, it cancels the background services.
                // This isn't an error condition, so don't log it as an error.
                if (stopCalled && backgroundTask.IsCanceled && ex is OperationCanceledException)
                {
                    return;
                }

                logger.BackgroundServiceFaulted(ex);
                if (options.BackgroundServiceExceptionBehavior == HostBackgroundServiceExceptionBehavior.StopHost)
                {
                    logger.BackgroundServiceStoppingHost(ex);
                    applicationLifetime.StopApplication();
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            stopCalled = true;
            logger.Stopping();

            using (var cts = new CancellationTokenSource(options.ShutdownTimeout))
            {
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken))
                {
                    var token = linkedCts.Token;

                    // Trigger IHostApplicationLifetime.ApplicationStopping
                    applicationLifetime.StopApplication();

                    var exceptions = new List<Exception>();

                    if (hostedServices != null) // Started?
                    {
                        foreach (IHostService hostedService in hostedServices.Reverse())
                        {
                            try
                            {
                                await hostedService.StopAsync(token).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                            }
                        }
                    }

                    // Fire IHostApplicationLifetime.Stopped
                    applicationLifetime.NotifyStopped();

                    try
                    {
                        await hostLifetime.StopAsync(token).ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }

                    if (exceptions.Count > 0)
                    {
                        var exception = new AggregateException("One or more hosted services failed to stop.", exceptions);
                        logger.StoppedWithException(exception);
                        throw exception;
                    }
                }

            }

            logger.Stopped();
        }

        public void Dispose() => 
            DisposeAsync().AsTask().GetAwaiter().GetResult();

        public async ValueTask DisposeAsync()
        {
            // The user didn't change the ContentRootFileProvider instance, we can dispose it
            if (ReferenceEquals(hostEnvironment.ContentRootFileProvider, fileProvider))
            {
                // Dispose the content provider
                await DisposeAsync(hostEnvironment.ContentRootFileProvider).ConfigureAwait(false);
            }
            else
            {
                // In the rare case that the user replaced the ContentRootFileProvider, dispose it and the one
                // we originally created
                await DisposeAsync(hostEnvironment.ContentRootFileProvider).ConfigureAwait(false);
                await DisposeAsync(fileProvider).ConfigureAwait(false);
            }

            // Dispose the service provider
            await DisposeAsync(Services).ConfigureAwait(false);

            static async ValueTask DisposeAsync(object o)
            {
                switch (o)
                {
                    case IAsyncDisposable asyncDisposable:
                        await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                        break;
                    case IDisposable disposable:
                        disposable.Dispose();
                        break;
                }
            }
        }
    }
}

