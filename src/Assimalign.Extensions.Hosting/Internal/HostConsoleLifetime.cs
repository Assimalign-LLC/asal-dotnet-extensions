
using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;


namespace Assimalign.Extensions.Hosting.Internal
{

    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Logging.Abstractions;
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Options.Abstractions;
    using Assimalign.Extensions.Hosting.Abstractions;
    using System.Diagnostics;
    using System.Runtime;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Listens for Ctrl+C or SIGTERM and initiates shutdown.
    /// </summary>
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public class HostConsoleLifetime : IHostLifetime, IDisposable
    {
        private CancellationTokenRegistration _applicationStartedRegistration;
        private CancellationTokenRegistration _applicationStoppingRegistration;
#if NET6_0_OR_GREATER
        private PosixSignalRegistration _sigIntRegistration;
        private PosixSignalRegistration _sigQuitRegistration;
        private PosixSignalRegistration _sigTermRegistration;
#else
        private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);

#endif

        public HostConsoleLifetime(IOptions<HostConsoleLifetimeOptions> options, IHostEnvironment environment, IHostApplicationLifetime applicationLifetime, IOptions<HostOptions> hostOptions)
            : this(options, environment, applicationLifetime, hostOptions, NullLoggerFactory.Instance) { }

        public HostConsoleLifetime(IOptions<HostConsoleLifetimeOptions> options, IHostEnvironment environment, IHostApplicationLifetime applicationLifetime, IOptions<HostOptions> hostOptions, ILoggerFactory loggerFactory)
        {
            Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            HostOptions = hostOptions?.Value ?? throw new ArgumentNullException(nameof(hostOptions));
            Logger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
        }

        private HostConsoleLifetimeOptions Options { get; }

        private IHostEnvironment Environment { get; }

        private IHostApplicationLifetime ApplicationLifetime { get; }

        private HostOptions HostOptions { get; }

        private ILogger Logger { get; }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            if (!Options.SuppressStatusMessages)
            {
                _applicationStartedRegistration = ApplicationLifetime.ApplicationStarted.Register(state =>
                {
                    ((HostConsoleLifetime)state).OnApplicationStarted();
                },
                this);
                _applicationStoppingRegistration = ApplicationLifetime.ApplicationStopping.Register(state =>
                {
                    ((HostConsoleLifetime)state).OnApplicationStopping();
                },
                this);
            }

            RegisterShutdownHandlers();

            // Console applications start immediately.
            return Task.CompletedTask;
        }

        private void RegisterShutdownHandlers()
        {
#if NET6_0_OR_GREATER
            Action<PosixSignalContext> handler = HandlePosixSignal;
            _sigIntRegistration = PosixSignalRegistration.Create(PosixSignal.SIGINT, handler);
            _sigQuitRegistration = PosixSignalRegistration.Create(PosixSignal.SIGQUIT, handler);
            _sigTermRegistration = PosixSignalRegistration.Create(PosixSignal.SIGTERM, handler);
#else
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Console.CancelKeyPress += OnCancelKeyPress;
#endif
        }
#if NET6_0_OR_GREATER
        private void HandlePosixSignal(PosixSignalContext context)
        {
            Debug.Assert(context.Signal == PosixSignal.SIGINT || context.Signal == PosixSignal.SIGQUIT || context.Signal == PosixSignal.SIGTERM);

            context.Cancel = true;
            ApplicationLifetime.StopApplication();
        }
#endif

        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            ApplicationLifetime.StopApplication();

            // Don't block in process shutdown for CTRL+C/SIGINT since we can set e.Cancel to true
            // we assume that application code will unwind once StopApplication signals the token
            _shutdownBlock.Set();
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            ApplicationLifetime.StopApplication();
            if (!_shutdownBlock.WaitOne(HostOptions.ShutdownTimeout))
            {
                Logger.LogInformation("Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.");
            }

            // wait one more time after the above log message, but only for ShutdownTimeout, so it doesn't hang forever
            _shutdownBlock.WaitOne(HostOptions.ShutdownTimeout);

            // On Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
            // Suppress that since we shut down gracefully. https://github.com/dotnet/aspnetcore/issues/6526
            System.Environment.ExitCode = 0;
        }

        private void OnApplicationStarted()
        {
            Logger.LogInformation("Application started. Press Ctrl+C to shut down.");
            Logger.LogInformation("Hosting environment: {envName}", Environment.EnvironmentName);
            Logger.LogInformation("Content root path: {contentRoot}", Environment.ContentRootPath);
        }

        private void OnApplicationStopping()
        {
            Logger.LogInformation("Application is shutting down...");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // There's nothing to do here
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            UnregisterShutdownHandlers();

            _applicationStartedRegistration.Dispose();
            _applicationStoppingRegistration.Dispose();
        }

        private void UnregisterShutdownHandlers()
        {
#if NET6_0_OR_GREATER
            _sigIntRegistration?.Dispose();
            _sigQuitRegistration?.Dispose();
            _sigTermRegistration?.Dispose();
#else
            _shutdownBlock.Set();

            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            Console.CancelKeyPress -= OnCancelKeyPress;
#endif
        }
    }
}
