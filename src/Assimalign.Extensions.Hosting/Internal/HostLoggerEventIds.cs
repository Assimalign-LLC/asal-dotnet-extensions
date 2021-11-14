using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Internal
{
    using Assimalign.Extensions.Logging;

    internal static class HostLoggerEventIds
    {
        public static readonly EventId Starting = new EventId(1, nameof(Starting));
        public static readonly EventId Started = new EventId(2, nameof(Started));
        public static readonly EventId Stopping = new EventId(3, nameof(Stopping));
        public static readonly EventId Stopped = new EventId(4, nameof(Stopped));
        public static readonly EventId StoppedWithException = new EventId(5, nameof(StoppedWithException));
        public static readonly EventId ApplicationStartupException = new EventId(6, nameof(ApplicationStartupException));
        public static readonly EventId ApplicationStoppingException = new EventId(7, nameof(ApplicationStoppingException));
        public static readonly EventId ApplicationStoppedException = new EventId(8, nameof(ApplicationStoppedException));
        public static readonly EventId BackgroundServiceFaulted = new EventId(9, nameof(BackgroundServiceFaulted));
        public static readonly EventId BackgroundServiceStoppingHost = new EventId(10, nameof(BackgroundServiceStoppingHost));
    }
}
