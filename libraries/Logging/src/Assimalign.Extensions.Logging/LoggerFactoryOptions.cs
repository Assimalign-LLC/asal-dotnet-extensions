using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging
{
    /// <summary>
    /// The options for a LoggerFactory.
    /// </summary>
    public class LoggerFactoryOptions
    {
        /// <summary>
        /// Creates a new <see cref="LoggerFactoryOptions"/> instance.
        /// </summary>
        public LoggerFactoryOptions() { }

        /// <summary>
        /// Gets or sets <see cref="LoggerFactoryOptions"/> value to indicate which parts of the tracing context information should be included with the logging scopes.
        /// </summary>
        public ActivityTrackingOptions ActivityTrackingOptions { get; set; }
    }
}
