using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging
{
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.Logging;

    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="ILoggingBuilder" />.
    /// </summary>
    public static class LoggingBuilderConfigurationExtensions
    {
        /// <summary>
        /// Adds services required to consume <see cref="ILoggerProviderConfigurationFactory"/> or <see cref="ILoggerProviderConfiguration{T}"/>
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to register services on.</param>
        public static void AddConfiguration(this ILoggingBuilder builder)
        {
            builder.Services.TryAddSingleton<ILoggerProviderConfigurationFactory, LoggerProviderConfigurationFactory>();
            builder.Services.TryAddSingleton(typeof(ILoggerProviderConfiguration<>), typeof(LoggerProviderConfiguration<>));
        }
    }
}
