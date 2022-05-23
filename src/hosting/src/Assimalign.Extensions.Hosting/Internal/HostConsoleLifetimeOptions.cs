using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting.Internal
{
    public class HostConsoleLifetimeOptions
    {
        /// <summary>
        /// Indicates if host lifetime status messages should be suppressed such as on startup.
        /// The default is false.
        /// </summary>
        public bool SuppressStatusMessages { get; set; }
    }
}
