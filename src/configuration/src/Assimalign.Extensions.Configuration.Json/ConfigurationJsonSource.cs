namespace Assimalign.Extensions.Configuration.Providers
{
    using Assimalign.Extensions.Configuration;

    /// <summary>
    /// Represents a JSON file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class ConfigurationJsonSource : ConfigurationFileSource
    {
        /// <summary>
        /// Builds the <see cref="ConfigurationJsonProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="ConfigurationJsonProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new ConfigurationJsonProvider(this);
        }
    }
}
