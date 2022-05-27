using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options.Abstractions
{
    /// <summary>
    /// Represents something that configures the <typeparamref name="TOptions"/> type.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IConfigureNamedOptions<in TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions"/> instance.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        void Configure(string name, TOptions options);
    }
}
