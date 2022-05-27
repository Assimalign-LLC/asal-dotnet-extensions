namespace Assimalign.Extensions.Logging
{
    using Assimalign.Extensions.Configuration;

    internal sealed class LoggingConfiguration
    {
        public IConfiguration Configuration { get; }

        public LoggingConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
