using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options.Abstractions
{
    /// <summary>
    /// Represents something that configures the <typeparamref name="TOptions"/> type.
    /// Note: These are run after all <see cref="IConfigureOptions{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions">Options type being configured.</typeparam>
    public interface IPostConfigureOptions<in TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions"/> instance.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configured.</param>
        void PostConfigure(string name, TOptions options);
    }
}
