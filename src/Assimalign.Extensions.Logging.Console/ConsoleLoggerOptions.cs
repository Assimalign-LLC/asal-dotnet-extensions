using System;

namespace Assimalign.Extensions.Logging.Console
{
    /// <summary>
    /// Options for a <see cref="ConsoleLogger"/>.
    /// </summary>
    public class ConsoleLoggerOptions
    {

        /// <summary>
        /// Name of the log message formatter to use. Defaults to "simple" />.
        /// </summary>
        public string FormatterName { get; set; }


        /// <summary>
        /// Gets or sets value indicating the minimum level of messages that would get written to <c>Console.Error</c>.
        /// </summary>
        public LogLevel LogToStandardErrorThreshold { get; set; } = LogLevel.None;
    }
}
