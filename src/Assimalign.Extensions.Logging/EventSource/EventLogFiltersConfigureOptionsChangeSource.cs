// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Assimalign.Extensions.Logging.EventSource;
using Assimalign.Extensions.Options.Abstractions;
using Assimalign.Extensions.Primitives.Abstractions;

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
