using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging
{
    using Assimalign.Extensions.Logging.Abstractions;
    using Assimalign.Extensions.DependencyInjection.Abstractions;

    internal sealed class LoggingBuilder : ILoggingBuilder
    {
        public LoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
