using System.Text.Json;

namespace Assimalign.Extensions.Logging.Console
{
    /// <summary>
    /// Options for the built-in json console log formatter.
    /// </summary>
    public class JsonConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public JsonConsoleFormatterOptions() { }

        /// <summary>
        /// Gets or sets JsonWriterOptions.
        /// </summary>
        public JsonWriterOptions JsonWriterOptions { get; set; }
    }
}
