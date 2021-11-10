// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



namespace Assimalign.Extensions.Logging.EventSource
{
    using Assimalign.Extensions.Logging.EventSource;
    using Assimalign.Extensions.Options.Abstractions;

    internal sealed class EventLogFiltersConfigureOptions : IConfigureOptions<LoggerFilterOptions>
    {
        private readonly LoggingEventSource _eventSource;

        public EventLogFiltersConfigureOptions(LoggingEventSource eventSource)
        {
            _eventSource = eventSource;
        }

        public void Configure(LoggerFilterOptions options)
        {
            foreach (LoggerFilterRule rule in _eventSource.GetFilterRules())
            {
                options.Rules.Add(rule);
            }
        }
    }
}
