using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging.Abstractions
{
    /// <summary>
    /// Represents a <see cref="ILoggerProvider"/> that is able to consume external scope information.
    /// </summary>
    public interface ISupportExternalScope
    {
        /// <summary>
        /// Sets external scope information source for logger provider.
        /// </summary>
        /// <param name="scopeProvider">The provider of scope data.</param>
        void SetScopeProvider(IExternalScopeProvider scopeProvider);
    }
}
