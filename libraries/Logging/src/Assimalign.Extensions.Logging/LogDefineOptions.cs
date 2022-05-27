using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging
{
    /// <summary>
    /// Options for <see cref="LoggerMessage.Define(LogLevel, EventId, string)"/> and its overloads
    /// </summary>
    public class LogDefineOptions
    {
        /// <summary>
        /// Gets or sets the flag to skip IsEnabled check for the logging method.
        /// </summary>
        public bool SkipEnabledCheck { get; set; }
    }
}
