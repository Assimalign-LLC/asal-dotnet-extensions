using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Internal
{
    using Assimalign.Extensions.Hosting.Abstractions;
    using Assimalign.Extensions.Logging.Abstractions;

    public class HostApplicationLifetime : IHostApplicationLifetime
    {
        private readonly CancellationTokenSource startedSource = new CancellationTokenSource();
        private readonly CancellationTokenSource stoppingSource = new CancellationTokenSource();
        private readonly CancellationTokenSource stoppedSource = new CancellationTokenSource();
        private readonly ILogger<HostApplicationLifetime> logger;

        public HostApplicationLifetime(ILogger<HostApplicationLifetime> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Triggered when the application host has fully started and is about to wait
        /// for a graceful shutdown.
        /// </summary>
        public CancellationToken ApplicationStarted => startedSource.Token;

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// Request may still be in flight. Shutdown will block until this event completes.
        /// </summary>
        public CancellationToken ApplicationStopping => stoppingSource.Token;

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// All requests should be complete at this point. Shutdown will block
        /// until this event completes.
        /// </summary>
        public CancellationToken ApplicationStopped => stoppedSource.Token;

        /// <summary>
        /// Signals the ApplicationStopping event and blocks until it completes.
        /// </summary>
        public void StopApplication()
        {
            // Lock on CTS to synchronize multiple calls to StopApplication. This guarantees that the first call
            // to StopApplication and its callbacks run to completion before subsequent calls to StopApplication,
            // which will no-op since the first call already requested cancellation, get a chance to execute.
            lock (stoppingSource)
            {
                try
                {
                    ExecuteHandlers(stoppingSource);
                }
                catch (Exception exception)
                {
                    logger.ApplicationError(
                        HostLoggerEventIds.ApplicationStoppingException,
                        "An error occurred stopping the application",
                        exception);
                }
            }
        }

        /// <summary>
        /// Signals the ApplicationStarted event and blocks until it completes.
        /// </summary>
        public void NotifyStarted()
        {
            try
            {
                ExecuteHandlers(startedSource);
            }
            catch (Exception exception)
            {
                logger.ApplicationError(
                    HostLoggerEventIds.ApplicationStartupException,
                    "An error occurred starting the application",
                    exception);
            }
        }

        /// <summary>
        /// Signals the ApplicationStopped event and blocks until it completes.
        /// </summary>
        public void NotifyStopped()
        {
            try
            {
                ExecuteHandlers(stoppedSource);
            }
            catch (Exception exception)
            {
                logger.ApplicationError(
                    HostLoggerEventIds.ApplicationStoppedException,
                    "An error occurred stopping the application",
                    exception);
            }
        }

        private void ExecuteHandlers(CancellationTokenSource cancel)
        {
            // Noop if this is already cancelled
            if (cancel.IsCancellationRequested)
            {
                return;
            }

            // Run the cancellation token callbacks
            cancel.Cancel(throwOnFirstException: false);
        }
    }
}
