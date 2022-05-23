using System;
using System.Diagnostics;
using System.Threading;

namespace Assimalign.Extensions.Net.Http.Internal;


// Thread-safety: We treat this class as immutable except for the timer. Creating a new object
// for the 'expiry' pool simplifies the threading requirements significantly.
internal sealed class HttpMessageHandlerActiveTrackingEntry
{
    private static readonly TimerCallback timerCallback = (s) => ((HttpMessageHandlerActiveTrackingEntry)s).TimerTick();
    private readonly object @lock;
    private bool isTimerInitialized;
    private Timer timer;
    private TimerCallback callback;

    public HttpMessageHandlerActiveTrackingEntry(string name, HttpMessageHandlerLifetimeTracking handler, TimeSpan lifetime)
    {
        Name = name;
        Handler = handler;
        Lifetime = lifetime;
        @lock = new object();
    }

    public HttpMessageHandlerLifetimeTracking Handler { get; private set; }
    public TimeSpan Lifetime { get; }
    public string Name { get; }
    public void StartExpiryTimer(TimerCallback callback)
    {
        if (Lifetime == Timeout.InfiniteTimeSpan)
        {
            return; // never expires.
        }
        if (Volatile.Read(ref isTimerInitialized))
        {
            return;
        }
        StartExpiryTimerSlow(callback);
    }
    private void StartExpiryTimerSlow(TimerCallback callback)
    {
        Debug.Assert(Lifetime != Timeout.InfiniteTimeSpan);
        lock (@lock)
        {
            if (Volatile.Read(ref isTimerInitialized))
            {
                return;
            }
            this.callback = callback;
            this.timer = NonCapturingTimer.Create(timerCallback, this, Lifetime, Timeout.InfiniteTimeSpan);
            this.isTimerInitialized = true;
        }
    }
    private void TimerTick()
    {
        Debug.Assert(this.callback != null);
        Debug.Assert(this.timer != null);

        lock (@lock)
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
                this.callback(this);
            }
        }
    }
}
