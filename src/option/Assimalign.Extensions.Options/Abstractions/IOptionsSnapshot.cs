using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options.Abstractions
{
    /// <summary>
    /// Used to access the value of <typeparamref name="TOptions"/> for the lifetime of a request.
    /// </summary>
    /// <typeparam name="TOptions">Options type.</typeparam>
    public interface IOptionsSnapshot<out TOptions> :
        IOptions<TOptions>
        where TOptions : class
    {
        /// <summary>
        /// Returns a configured <typeparamref name="TOptions"/> instance with the given name.
        /// </summary>
        TOptions Get(string name);
    }
}
