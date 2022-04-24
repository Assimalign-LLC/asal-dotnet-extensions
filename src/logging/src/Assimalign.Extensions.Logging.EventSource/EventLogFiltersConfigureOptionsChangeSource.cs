using Assimalign.Extensions.Logging.EventSource;
using Assimalign.Extensions.Options;
using Assimalign.Extensions.Primitives;

namespace Assimalign.Extensions.Logging
{
    internal sealed class EventLogFiltersConfigureOptionsChangeSource: IOptionsChangeTokenSource<LoggerFilterOptions>
    {
        private readonly LoggingEventSource _eventSource;

        public EventLogFiltersConfigureOptionsChangeSource(LoggingEventSource eventSource)
        {
            _eventSource = eventSource;
        }

        public IChangeToken GetChangeToken() => _eventSource.GetFilterChangeToken();

        public string Name { get; }
    }
}
