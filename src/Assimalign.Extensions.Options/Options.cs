using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Options.Abstractions;

    public class Options<TOptions> : IOptions<TOptions>
        where TOptions : class
    {
        /// <summary>
        /// Initializes the wrapper with the options instance to return.
        /// </summary>
        /// <param name="options">The options instance to return.</param>
        public Options(TOptions options)
        {
            Value = options;
        }

        /// <summary>
        /// The options instance.
        /// </summary>
        public TOptions Value { get; }


        /// <summary>
        /// The default name used for options instances: "".
        /// </summary>
        public static readonly string DefaultName = string.Empty;

        /// <summary>
        /// Creates a wrapper around an instance of <typeparamref name="TOptions"/> to return itself as an <see cref="IOptions{TOptions}"/>.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <param name="options">Options object.</param>
        /// <returns>Wrapped options object.</returns>
        public static IOptions<TOptions> Create(TOptions options) => 
            new Options.Options<TOptions>(options);
    }
}
