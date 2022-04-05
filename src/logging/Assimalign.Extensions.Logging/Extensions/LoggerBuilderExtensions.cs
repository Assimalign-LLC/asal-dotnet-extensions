using System;

namespace Assimalign.Extensions.Logging
{
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Options.Abstractions;
    using Assimalign.Extensions.Logging.Abstractions;
    using Assimalign.Extensions.DependencyInjection;

    public static class LoggerBuilderExtensions
    {
        /// <summary>
        /// Sets a minimum <see cref="LogLevel"/> requirement for log messages to be logged.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to set the minimum level on.</param>
        /// <param name="level">The <see cref="LogLevel"/> to set as the minimum.</param>
        /// <returns>The <see cref="ILoggingBuilder"/> so that additional calls can be chained.</returns>
        public static ILoggingBuilder SetMinimumLevel(this ILoggingBuilder builder, LogLevel level)
        {
            builder.Services.Add(ServiceDescriptor.Singleton<IConfigureOptions<LoggerFilterOptions>>(
                new LoggerLevelConfigureOptionsDefault(level)));
            return builder;
        }

        /// <summary>
        /// Adds the given <see cref="ILoggerProvider"/> to the <see cref="ILoggingBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to add the <paramref name="provider"/> to.</param>
        /// <param name="provider">The <see cref="ILoggerProvider"/> to add to the <paramref name="builder"/>.</param>
        /// <returns>The <see cref="ILoggingBuilder"/> so that additional calls can be chained.</returns>
        public static ILoggingBuilder AddProvider(this ILoggingBuilder builder, ILoggerProvider provider)
        {
            builder.Services.AddSingleton(provider);
            return builder;
        }

        /// <summary>
        /// Removes all <see cref="ILoggerProvider"/>s from <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to remove <see cref="ILoggerProvider"/>s from.</param>
        /// <returns>The <see cref="ILoggingBuilder"/> so that additional calls can be chained.</returns>
        public static ILoggingBuilder ClearProviders(this ILoggingBuilder builder)
        {
            builder.Services.RemoveAll<ILoggerProvider>();
            return builder;
        }

        /// <summary>
        /// Configure the <paramref name="builder"/> with the <see cref="LoggerFactoryOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to be configured with <see cref="LoggerFactoryOptions"/></param>
        /// <param name="action">The action used to configure the logger factory</param>
        /// <returns>The <see cref="ILoggingBuilder"/> so that additional calls can be chained.</returns>
        public static ILoggingBuilder Configure(this ILoggingBuilder builder, Action<LoggerFactoryOptions> action)
        {
            builder.Services.Configure(action);
            return builder;
        }
    }
}
