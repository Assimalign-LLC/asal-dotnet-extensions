using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging.Configuration
{
    using Assimalign.Extensions.Configuration.Abstractions;

    internal sealed class LoggerProviderConfiguration<T> : ILoggerProviderConfiguration<T>
    {
        public LoggerProviderConfiguration(ILoggerProviderConfigurationFactory providerConfigurationFactory)
        {
            Configuration = providerConfigurationFactory.GetConfiguration(typeof(T));
        }

        public IConfiguration Configuration { get; }
    }
}
