using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Assimalign.Extensions.Logging.Console
{
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Logging.Abstractions;
    using Assimalign.Extensions.Options.Abstractions;

    /// <summary>
    /// A provider of <see cref="ConsoleLogger"/> instances.
    /// </summary>
    [UnsupportedOSPlatform("browser")]
    [ProviderAlias("Console")]
    public class ConsoleLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly IOptionsMonitor<ConsoleLoggerOptions> _options;
        private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers;
        private ConcurrentDictionary<string, ConsoleFormatter> _formatters;
        private readonly ConsoleLoggerProcessor _messageQueue;

        private IDisposable _optionsReloadToken;
        private IExternalScopeProvider _scopeProvider = NullExternalScopeProvider.Instance;

        /// <summary>
        /// Creates an instance of <see cref="ConsoleLoggerProvider"/>.
        /// </summary>
        /// <param name="options">The options to create <see cref="ConsoleLogger"/> instances with.</param>
        public ConsoleLoggerProvider(IOptionsMonitor<ConsoleLoggerOptions> options)
            : this(options, Array.Empty<ConsoleFormatter>()) { }

        /// <summary>
        /// Creates an instance of <see cref="ConsoleLoggerProvider"/>.
        /// </summary>
        /// <param name="options">The options to create <see cref="ConsoleLogger"/> instances with.</param>
        /// <param name="formatters">Log formatters added for <see cref="ConsoleLogger"/> insteaces.</param>
        public ConsoleLoggerProvider(IOptionsMonitor<ConsoleLoggerOptions> options, IEnumerable<ConsoleFormatter> formatters)
        {
            _options = options;
            _loggers = new ConcurrentDictionary<string, ConsoleLogger>();
            SetFormatters(formatters);

            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = _options.OnChange(ReloadLoggerOptions);

            _messageQueue = new ConsoleLoggerProcessor();

            if (DoesConsoleSupportAnsi())
            {
                _messageQueue.Console = new AnsiLogConsole();
                _messageQueue.ErrorConsole = new AnsiLogConsole(stdErr: true);
            }
            else
            {
                _messageQueue.Console = new AnsiParsingLogConsole();
                _messageQueue.ErrorConsole = new AnsiParsingLogConsole(stdErr: true);
            }
        }

        //[UnsupportedOSPlatformGuard("windows")]
        private static bool DoesConsoleSupportAnsi()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return true;
            }

            // for Windows, check the console mode
            var stdOutHandle = Interop.Kernel32.GetStdHandle(Interop.Kernel32.STD_OUTPUT_HANDLE);
            if (!Interop.Kernel32.GetConsoleMode(stdOutHandle, out int consoleMode))
            {
                return false;
            }

            return (consoleMode & Interop.Kernel32.ENABLE_VIRTUAL_TERMINAL_PROCESSING) == Interop.Kernel32.ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        }

        private void SetFormatters(IEnumerable<ConsoleFormatter> formatters = null)
        {
            var cd = new ConcurrentDictionary<string, ConsoleFormatter>(StringComparer.OrdinalIgnoreCase);

            bool added = false;
            if (formatters != null)
            {
                foreach (ConsoleFormatter formatter in formatters)
                {
                    cd.TryAdd(formatter.Name, formatter);
                    added = true;
                }
            }

            if (!added)
            {
                cd.TryAdd(ConsoleFormatterNames.Simple, new SimpleConsoleFormatter(new FormatterOptionsMonitor<SimpleConsoleFormatterOptions>(new SimpleConsoleFormatterOptions())));
                cd.TryAdd(ConsoleFormatterNames.Systemd, new SystemdConsoleFormatter(new FormatterOptionsMonitor<ConsoleFormatterOptions>(new ConsoleFormatterOptions())));
                cd.TryAdd(ConsoleFormatterNames.Json, new JsonConsoleFormatter(new FormatterOptionsMonitor<JsonConsoleFormatterOptions>(new JsonConsoleFormatterOptions())));
            }

            _formatters = cd;
        }

        // warning:  ReloadLoggerOptions can be called before the ctor completed,... before registering all of the state used in this method need to be initialized
        private void ReloadLoggerOptions(ConsoleLoggerOptions options)
        {
            if (!_formatters.TryGetValue(_options.CurrentValue.FormatterName, out ConsoleFormatter logFormatter))
            {
                logFormatter = _formatters[ConsoleFormatterNames.Systemd];
            }

            foreach (KeyValuePair<string, ConsoleLogger> logger in _loggers)
            {
                logger.Value.Options = options;
                logger.Value.Formatter = logFormatter;
            }
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            if (!_formatters.TryGetValue(_options.CurrentValue.FormatterName, out ConsoleFormatter logFormatter))
            {
                logFormatter = _formatters[ConsoleFormatterNames.Systemd];
            }

            return _loggers.TryGetValue(name, out ConsoleLogger logger) ?
                logger :
                _loggers.GetOrAdd(name, new ConsoleLogger(name, _messageQueue)
                {
                    Options = _options.CurrentValue,
                    ScopeProvider = _scopeProvider,
                    Formatter = logFormatter,
                });
        }


        /// <inheritdoc />
        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _messageQueue.Dispose();
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;

            foreach (System.Collections.Generic.KeyValuePair<string, ConsoleLogger> logger in _loggers)
            {
                logger.Value.ScopeProvider = _scopeProvider;
            }
        }
    }
}
