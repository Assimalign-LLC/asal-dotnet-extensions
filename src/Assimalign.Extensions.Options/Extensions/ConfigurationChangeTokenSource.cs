using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Configuration.Abstractions;
    using Assimalign.Extensions.Primitives.Abstractions;
    using Assimalign.Extensions.Options.Abstractions;

    /// <summary>
    /// Creates <see cref="IChangeToken"/>s so that <see cref="IOptionsMonitor{TOptions}"/> gets
    /// notified when <see cref="IConfiguration"/> changes.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public class ConfigurationChangeTokenSource<TOptions> : IOptionsChangeTokenSource<TOptions>
        where TOptions : class
    {
        private IConfiguration _config;

        /// <summary>
        /// Constructor taking the <see cref="IConfiguration"/> instance to watch.
        /// </summary>
        /// <param name="config">The configuration instance.</param>
        public ConfigurationChangeTokenSource(IConfiguration config) : this(Options<TOptions>.DefaultName, config)
        { }

        /// <summary>
        /// Constructor taking the <see cref="IConfiguration"/> instance to watch.
        /// </summary>
        /// <param name="name">The name of the options instance being watched.</param>
        /// <param name="config">The configuration instance.</param>
        public ConfigurationChangeTokenSource(string name, IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            _config = config;
            Name = name ?? Options<TOptions>.DefaultName;
        }

        /// <summary>
        /// The name of the option instance being changed.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the reloadToken from the <see cref="IConfiguration"/>.
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetChangeToken()
        {
            return _config.GetReloadToken();
        }
    }
}