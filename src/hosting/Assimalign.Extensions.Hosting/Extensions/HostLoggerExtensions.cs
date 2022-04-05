using System;
using System.Reflection;


namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Logging.Abstractions;
    using Assimalign.Extensions.Hosting.Internal;

    internal static class HostLoggerExtensions
    {
        public static void ApplicationError(this ILogger logger, EventId eventId, string message, Exception exception)
        {
            if (exception is ReflectionTypeLoadException reflectionTypeLoadException)
            {
                foreach (Exception ex in reflectionTypeLoadException.LoaderExceptions)
                {
                    message = message + Environment.NewLine + ex.Message;
                }
            }

            logger.LogCritical(
                eventId: eventId,
                message: message,
                exception: exception);
        }

        public static void Starting(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                   eventId: HostLoggerEventIds.Starting,
                   message: "Hosting starting");
            }
        }

        public static void Started(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                    eventId: HostLoggerEventIds.Started,
                    message: "Hosting started");
            }
        }

        public static void Stopping(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                    eventId: HostLoggerEventIds.Stopping,
                    message: "Hosting stopping");
            }
        }

        public static void Stopped(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                    eventId: HostLoggerEventIds.Stopped,
                    message: "Hosting stopped");
            }
        }

        public static void StoppedWithException(this ILogger logger, Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                    eventId: HostLoggerEventIds.StoppedWithException,
                    exception: ex,
                    message: "Hosting shutdown exception");
            }
        }

        public static void BackgroundServiceFaulted(this ILogger logger, Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(
                    eventId: HostLoggerEventIds.BackgroundServiceFaulted,
                    exception: ex,
                    message: "BackgroundService failed");
            }
        }

        public static void BackgroundServiceStoppingHost(this ILogger logger, Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Critical))
            {
                logger.LogCritical(
                    eventId: HostLoggerEventIds.BackgroundServiceStoppingHost,
                    exception: ex,
                    message: "unknown right now");// SR.BackgroundServiceExceptionStoppedHost);
            }
        }
    }
}

