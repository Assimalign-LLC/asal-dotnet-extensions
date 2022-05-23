using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Options.Abstractions
{
    /// <summary>
    /// Used to retrieve configured <typeparamref name="TOptions"/> instances.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being requested.</typeparam>
    public interface IOptions<out TOptions>
        where TOptions : class
    {
        /// <summary>
        /// The default configured <typeparamref name="TOptions"/> instance
        /// </summary>
        TOptions Value { get; }
     
    }
}

