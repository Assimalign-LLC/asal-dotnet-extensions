using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options.Abstractions
{
    /// <summary>
    /// Represents something that configures the <typeparamref name="TOptions"/> type.
    /// Note: These are run before all <see cref="IPostConfigureOptions{TOptions}"/>.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IConfigureOptions<in TOptions> where TOptions : class
    {
        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions"/> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        void Configure(TOptions options);
    }
}
