using System;
using System.Net.Http;
using System.Threading;
using System.Collections.Generic;


namespace Assimalign.Extensions.Net.Http;

/// <summary>
/// An options class for configuring the default <see cref="IHttpClientFactory"/>.
/// </summary>
public sealed class HttpClientFactoryOptions
{
    // Establishing a minimum lifetime helps us avoid some possible destructive cases.
    //
    // IMPORTANT: This is used in a resource string. Update the resource if this changes.
    internal static readonly TimeSpan MinimumHandlerLifetime = TimeSpan.FromSeconds(1);

    private TimeSpan httpMessageHandlerLifetime = TimeSpan.FromMinutes(2);

    private readonly IList<Action<HttpClient>> httpClientActions;
    private readonly IList<Action<HttpMessageHandlerBuilder>> httpMessageHandlerBuilderActions;

    public HttpClientFactoryOptions()
    {
        this.httpClientActions = new List<Action<HttpClient>>();
        this.httpMessageHandlerBuilderActions = new List<Action<HttpMessageHandlerBuilder>>();
    }

    /// <summary>
    /// Gets a list of operations used to configure an <see cref="HttpMessageHandlerBuilder"/>.
    /// </summary>
    public IEnumerable<Action<HttpMessageHandlerBuilder>> HttpMessageHandlerBuilderActions => this.httpMessageHandlerBuilderActions;

    /// <summary>
    /// Gets a list of operations used to configure an <see cref="HttpClient"/>.
    /// </summary>
    public IEnumerable<Action<HttpClient>> HttpClientActions => this.httpClientActions;

    /// <summary>
    /// Gets or sets the length of time that a <see cref="HttpMessageHandler"/> instance can be reused. Each named
    /// client can have its own configured handler lifetime value. The default value of this property is two minutes.
    /// Set the lifetime to <see cref="Timeout.InfiniteTimeSpan"/> to disable handler expiry.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default implementation of <see cref="IHttpClientFactory"/> will pool the <see cref="HttpMessageHandler"/>
    /// instances created by the factory to reduce resource consumption. This setting configures the amount of time
    /// a handler can be pooled before it is scheduled for removal from the pool and disposal.
    /// </para>
    /// <para>
    /// Pooling of handlers is desirable as each handler typically manages its own underlying HTTP connections; creating
    /// more handlers than necessary can result in connection delays. Some handlers also keep connections open indefinitely
    /// which can prevent the handler from reacting to DNS changes. The value of <see cref="HandlerLifetime"/> should be
    /// chosen with an understanding of the application's requirement to respond to changes in the network environment.
    /// </para>
    /// <para>
    /// Expiry of a handler will not immediately dispose the handler. An expired handler is placed in a separate pool
    /// which is processed at intervals to dispose handlers only when they become unreachable. Using long-lived
    /// <see cref="HttpClient"/> instances will prevent the underlying <see cref="HttpMessageHandler"/> from being
    /// disposed until all references are garbage-collected.
    /// </para>
    /// </remarks>
    public TimeSpan HandlerLifetime
    {
        get => httpMessageHandlerLifetime;
        set
        {
            if (value != Timeout.InfiniteTimeSpan && value < MinimumHandlerLifetime)
            {
                throw new ArgumentException();// SR.HandlerLifetime_InvalidValue, nameof(value));
            }

            httpMessageHandlerLifetime = value;
        }
    }


    public HttpClientFactoryOptions AddHttpClientAction(Action<HttpClient> action)
    {
        this.httpClientActions.Add(action);
        return this;
    } 
    public HttpClientFactoryOptions AddHttpHandlerBuilderAction(Action<HttpMessageHandlerBuilder> action)
    {
        this.httpMessageHandlerBuilderActions.Add(action);
        return this;
    }
}
