using System;
using System.Net.Http;

namespace Assimalign.Extensions.Net.Http.Internal;

internal class HttpMessageHandlerExpiredTrackingEntry
{
    private readonly WeakReference livenessTracker;

    // IMPORTANT: don't cache a reference to `other` or `other.Handler` here. We need to allow it to be GC'ed.
    public HttpMessageHandlerExpiredTrackingEntry(HttpMessageHandlerActiveTrackingEntry other)
    {
        Name = other.Name;
        livenessTracker = new WeakReference(other.Handler);
        InnerHandler = other.Handler.InnerHandler;
    }
    public bool CanDispose => !livenessTracker.IsAlive;
    public HttpMessageHandler InnerHandler { get; }
    public string Name { get; }
}
