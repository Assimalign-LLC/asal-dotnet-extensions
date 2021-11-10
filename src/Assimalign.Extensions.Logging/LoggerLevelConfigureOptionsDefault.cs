using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Logging
{
    using Assimalign.Extensions.Options;

    internal sealed class LoggerLevelConfigureOptionsDefault : ConfigureOptions<LoggerFilterOptions>
    {
        public LoggerLevelConfigureOptionsDefault(LogLevel level) : base(options => options.MinLevel = level)
        {
        }
    }
}
