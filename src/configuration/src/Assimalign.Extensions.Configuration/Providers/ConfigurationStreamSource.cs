using System;
using System.IO;

namespace Assimalign.Extensions.Configuration.Providers;

using Assimalign.Extensions.Configuration;

/// <summary>
/// Stream based <see cref="IConfigurationSource" />.
/// </summary>
public abstract class ConfigurationStreamSource : IConfigurationSource
{
    /// <summary>
    /// The stream containing the configuration data.
    /// </summary>
    public Stream Stream { get; set; }

    /// <summary>
    /// Builds the <see cref="ConfigurationStreamProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>An <see cref="IConfigurationProvider"/></returns>
    public abstract IConfigurationProvider Build(IConfigurationBuilder builder);
}
