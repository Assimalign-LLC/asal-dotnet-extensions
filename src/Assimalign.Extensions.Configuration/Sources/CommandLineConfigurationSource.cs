using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.Configuration.Sources
{
    using Assimalign.Extensions.Configuration.Providers;
    using Assimalign.Extensions.Configuration.Abstractions;

    /// <summary>
    /// Represents command line arguments as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class CommandLineConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Gets or sets the switch mappings.
        /// </summary>
        public IDictionary<string, string> SwitchMappings { get; set; }

        /// <summary>
        /// Gets or sets the command line args.
        /// </summary>
        public IEnumerable<string> Args { get; set; }

        /// <summary>
        /// Builds the <see cref="CommandLineConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="CommandLineConfigurationProvider"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CommandLineConfigurationProvider(Args, SwitchMappings);
        }
    }
}
