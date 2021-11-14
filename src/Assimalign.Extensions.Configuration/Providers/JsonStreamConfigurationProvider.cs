using System.IO;

namespace Assimalign.Extensions.Configuration.Providers
{
    using Assimalign.Extensions.Configuration.Sources;

    /// <summary>
    /// Loads configuration key/values from a json stream into a provider.
    /// </summary>
    public class JsonStreamConfigurationProvider : StreamConfigurationProvider
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">The <see cref="JsonStreamConfigurationSource"/>.</param>
        public JsonStreamConfigurationProvider(JsonStreamConfigurationSource source) : base(source) { }

        /// <summary>
        /// Loads json configuration key/values from a stream into a provider.
        /// </summary>
        /// <param name="stream">The json <see cref="Stream"/> to load configuration data from.</param>
        public override void Load(Stream stream)
        {
            Data = JsonConfigurationProvider.JsonConfigurationFileParser.Parse(stream);
        }
    }
}
