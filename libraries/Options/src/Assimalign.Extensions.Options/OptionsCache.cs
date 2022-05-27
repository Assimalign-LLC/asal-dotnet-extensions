using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Options;

    /// <summary>
    /// Used to cache <typeparamref name="TOptions"/> instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public class OptionsCache<TOptions> : IOptionsMonitorCache<TOptions>
        where TOptions : class
    {
        private readonly ConcurrentDictionary<string, Lazy<TOptions>> _cache = new ConcurrentDictionary<string, Lazy<TOptions>>(concurrencyLevel: 1, capacity: 31, StringComparer.Ordinal); // 31 == default capacity

        /// <summary>
        /// Clears all options instances from the cache.
        /// </summary>
        public void Clear() => _cache.Clear();

        /// <summary>
        /// Gets a named options instance, or adds a new instance created with <paramref name="createOptions"/>.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="createOptions">The func used to create the new instance.</param>
        /// <returns>The options instance.</returns>
        public virtual TOptions GetOrAdd(string name, Func<TOptions> createOptions)
        {
            if (createOptions == null)
            {
                throw new ArgumentNullException(nameof(createOptions));
            }

            name = name ?? Options<TOptions>.DefaultName;
            Lazy<TOptions> value;

            if (!_cache.TryGetValue(name, out value))
            {
                value = _cache.GetOrAdd(name, new Lazy<TOptions>(createOptions));
            }

            return value.Value;
        }

        /// <summary>
        /// Gets a named options instance, if available.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>true if the options were retrieved; otherwise, false.</returns>
        internal bool TryGetValue(string name, out TOptions options)
        {
            if (_cache.TryGetValue(name ?? Options<TOptions>.DefaultName, out Lazy<TOptions> lazy))
            {
                options = lazy.Value;
                return true;
            }

            options = default;
            return false;
        }

        /// <summary>
        /// Tries to adds a new option to the cache, will return false if the name already exists.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="options">The options instance.</param>
        /// <returns>Whether anything was added.</returns>
        public virtual bool TryAdd(string name, TOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return _cache.TryAdd(name ?? Options<TOptions>.DefaultName, new Lazy<TOptions>(options));
        }

        /// <summary>
        /// Try to remove an options instance.
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <returns>Whether anything was removed.</returns>
        public virtual bool TryRemove(string name) =>
            _cache.TryRemove(name ?? Options<TOptions>.DefaultName, out _);
    }
}
