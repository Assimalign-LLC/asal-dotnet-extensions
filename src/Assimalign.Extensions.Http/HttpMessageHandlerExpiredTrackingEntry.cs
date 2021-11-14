using System;
using System.Net.Http;


namespace Assimalign.Extensions.Http
{
    using Assimalign.Extensions.DependencyInjection.Abstractions;

    // Thread-safety: This class is immutable
    internal sealed class HttpMessageHandlerExpiredTrackingEntry
    {
        private readonly WeakReference _livenessTracker;

        // IMPORTANT: don't cache a reference to `other` or `other.Handler` here.
        // We need to allow it to be GC'ed.
        public HttpMessageHandlerExpiredTrackingEntry(ActiveHandlerTrackingEntry other)
        {
            Name = other.Name;
            Scope = other.Scope;

            _livenessTracker = new WeakReference(other.Handler);
            InnerHandler = other.Handler.InnerHandler;
        }

        public bool CanDispose => !_livenessTracker.IsAlive;

        public HttpMessageHandler InnerHandler { get; }

        public string Name { get; }

        public IServiceScope Scope { get; }
    }
}
