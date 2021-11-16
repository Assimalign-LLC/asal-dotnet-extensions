using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.Configuration.Providers
{
    using Assimalign.Extensions.Configuration.Abstractions;

    /// <summary>
    /// Represents in-memory data as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class ConfigurationMemorySource : IConfigurationSource
    {
        /// <summary>
        /// The initial key value configuration pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> InitialData { get; set; }

        /// <summary>
        /// Builds the <see cref="ConfigurationMemoryProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="ConfigurationMemoryProvider"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigurationMemoryProvider(this);
        }
    }
}
