using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Options.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IOptions{TOptions}"/> and <see cref="IOptionsSnapshot{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type.</typeparam>
    public class OptionsManager<TOptions> :
        IOptions<TOptions>,
        IOptionsSnapshot<TOptions>
        where TOptions : class
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private readonly OptionsCache<TOptions> _cache = new OptionsCache<TOptions>(); // Note: this is a private cache

        /// <summary>
        /// Initializes a new instance with the specified options configurations.
        /// </summary>
        /// <param name="factory">The factory to use to create options.</param>
        public OptionsManager(IOptionsFactory<TOptions> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// The default configured <typeparamref name="TOptions"/> instance, equivalent to Get(Options.DefaultName).
        /// </summary>
        public TOptions Value => Get(Options<TOptions>.DefaultName);

        /// <summary>
        /// Returns a configured <typeparamref name="TOptions"/> instance with the given <paramref name="name"/>.
        /// </summary>
        public virtual TOptions Get(string name)
        {
            name = name ?? Options<TOptions>.DefaultName;

            if (!_cache.TryGetValue(name, out TOptions options))
            {
                // Store the options in our instance cache. Avoid closure on fast path by storing state into scoped locals.
                IOptionsFactory<TOptions> localFactory = _factory;
                string localName = name;
                options = _cache.GetOrAdd(name, () => localFactory.Create(localName));
            }

            return options;
        }
    }
}
