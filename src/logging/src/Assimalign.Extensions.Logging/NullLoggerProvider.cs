using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging
{
    using Assimalign.Extensions.Logging;

    /// <summary>
    /// Provider for the <see cref="NullLogger"/>.
    /// </summary>
    public class NullLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Returns an instance of <see cref="NullLoggerProvider"/>.
        /// </summary>
        public static NullLoggerProvider Instance { get; } = new NullLoggerProvider();

        private NullLoggerProvider()
        {
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return NullLogger.Instance;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
