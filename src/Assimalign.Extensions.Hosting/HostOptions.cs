using System;
using System.Globalization;


namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Hosting.Abstractions;
    using Assimalign.Extensions.Configuration.Abstractions;
    

    /// <summary>
    /// Options for <see cref="IHost"/>
    /// </summary>
    public class HostOptions
    {
        /// <summary>
        /// The default timeout for <see cref="IHost.StopAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        public TimeSpan ShutdownTimeout { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// The behavior the <see cref="IHost"/> will follow when any of
        /// its <see cref="BackgroundService"/> instances throw an unhandled exception.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="BackgroundServiceExceptionBehavior.StopHost"/>.
        /// </remarks>
        public HostBackgroundServiceExceptionBehavior BackgroundServiceExceptionBehavior { get; set; } =
            HostBackgroundServiceExceptionBehavior.StopHost;

        internal void Initialize(IConfiguration configuration)
        {
            var timeoutSeconds = configuration["shutdownTimeoutSeconds"];
            if (!string.IsNullOrEmpty(timeoutSeconds)
                && int.TryParse(timeoutSeconds, NumberStyles.None, CultureInfo.InvariantCulture, out var seconds))
            {
                ShutdownTimeout = TimeSpan.FromSeconds(seconds);
            }
        }
    }
}
