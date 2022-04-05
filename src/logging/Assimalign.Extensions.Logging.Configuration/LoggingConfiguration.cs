namespace Assimalign.Extensions.Logging
{
    using Assimalign.Extensions.Configuration.Abstractions;

    internal sealed class LoggingConfiguration
    {
        public IConfiguration Configuration { get; }

        public LoggingConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
