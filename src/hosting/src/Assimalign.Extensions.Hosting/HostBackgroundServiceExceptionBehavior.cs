using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Hosting;

    /// <summary>
    /// Specifies a behavior that the <see cref="IHost"/> will honor if an
    /// unhandled exception occurs in one of its <see cref="HostBackgroundService"/> instances.
    /// </summary>
    public enum HostBackgroundServiceExceptionBehavior
    {
        /// <summary>
        /// Stops the <see cref="IHost"/> instance.
        /// </summary>
        /// <remarks>
        /// If a <see cref="BackgroundService"/> throws an exception, the <see cref="IHost"/> instance stops, and the process continues.
        /// </remarks>
        StopHost = 0,

        /// <summary>
        /// Ignore exceptions thrown in <see cref="HostBackgroundService"/>.
        /// </summary>
        /// <remarks>
        /// If a <see cref="HostBackgroundService"/> throws an exception, the <see cref="IHost"/> will log the error, but otherwise ignore it.
        /// The <see cref="HostBackgroundService"/> is not restarted.
        /// </remarks>
        Ignore = 1
    }
}
