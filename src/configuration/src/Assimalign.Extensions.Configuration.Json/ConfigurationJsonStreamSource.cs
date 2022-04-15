namespace Assimalign.Extensions.Configuration.Providers
{
    using Assimalign.Extensions.Configuration;

    /// <summary>
    /// Represents a JSON file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class ConfigurationJsonStreamSource : ConfigurationStreamSource
    {
        /// <summary>
        /// Builds the <see cref="ConfigurationJsonStreamProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>An <see cref="ConfigurationJsonStreamProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
            => new ConfigurationJsonStreamProvider(this);
    }
}
