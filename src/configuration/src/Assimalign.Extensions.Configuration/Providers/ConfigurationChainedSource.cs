using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Configuration.Providers;

using Assimalign.Extensions.Configuration;


/// <summary>
/// Represents a chained <see cref="IConfiguration"/> as an <see cref="IConfigurationSource"/>.
/// </summary>
public class ConfigurationChainedSource : IConfigurationSource
{
    /// <summary>
    /// The chained configuration.
    /// </summary>
    public IConfiguration Configuration { get; set; }

    /// <summary>
    /// Whether the chained configuration should be disposed when the
    /// configuration provider gets disposed.
    /// </summary>
    public bool ShouldDisposeConfiguration { get; set; }

    /// <summary>
    /// Builds the <see cref="ConfigurationChainedProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>A <see cref="ConfigurationChainedProvider"/></returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => new ConfigurationChainedProvider(this);
}
