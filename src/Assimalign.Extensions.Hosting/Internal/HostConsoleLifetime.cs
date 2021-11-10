
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


    /// <summary>
    /// Listens for Ctrl+C or SIGTERM and initiates shutdown.
    /// </summary>
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public class HostConsoleLifetime : IHostLifetime, IDisposable
    {
		private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(initialState: false);

		private CancellationTokenRegistration _applicationStartedRegistration;

		private CancellationTokenRegistration _applicationStoppingRegistration;

		private HostConsoleLifetimeOptions Options { get; }

		private IHostEnvironment Environment { get; }

		private IHostApplicationLifetime ApplicationLifetime { get; }

		private HostOptions HostOptions { get; }

		private ILogger Logger { get; }

		public HostConsoleLifetime(
			IOptions<HostConsoleLifetimeOptions> options, 
			IHostEnvironment environment, 
			IHostApplicationLifetime applicationLifetime, 
			IOptions<HostOptions> hostOptions)
			: this(options, environment, applicationLifetime, hostOptions, NullLoggerFactory.Instance)
		{

		}

		public HostConsoleLifetime(
			IOptions<HostConsoleLifetimeOptions> options, 
			IHostEnvironment environment, 
			IHostApplicationLifetime applicationLifetime, 
			IOptions<HostOptions> hostOptions, 
			ILoggerFactory loggerFactory)
		{
			Options = options?.Value ?? throw new ArgumentNullException("options");
			Environment = environment ?? throw new ArgumentNullException("environment");
			ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException("applicationLifetime");
			HostOptions = hostOptions?.Value ?? throw new ArgumentNullException("hostOptions");
			Logger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
		}

		public Task WaitForStartAsync(CancellationToken cancellationToken)
		{
			if (!Options.SuppressStatusMessages)
			{
				_applicationStartedRegistration = ApplicationLifetime.ApplicationStarted.Register(delegate (object state)
				{
					((HostConsoleLifetime)state).OnApplicationStarted();
				}, this);
				_applicationStoppingRegistration = ApplicationLifetime.ApplicationStopping.Register(delegate (object state)
				{
					((HostConsoleLifetime)state).OnApplicationStopping();
				}, this);
			}
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
			Console.CancelKeyPress += OnCancelKeyPress;
			return Task.CompletedTask;
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

		private void OnProcessExit(object sender, EventArgs e)
		{
			ApplicationLifetime.StopApplication();
			if (!_shutdownBlock.WaitOne(HostOptions.ShutdownTimeout))
			{
				Logger.LogInformation("Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.");
			}
			_shutdownBlock.WaitOne();
			System.Environment.ExitCode = 0;
		}

		private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			e.Cancel = true;
			ApplicationLifetime.StopApplication();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_shutdownBlock.Set();
			AppDomain.CurrentDomain.ProcessExit -= new EventHandler(OnProcessExit);
			Console.CancelKeyPress -= OnCancelKeyPress;
			_applicationStartedRegistration.Dispose();
			_applicationStoppingRegistration.Dispose();
		}
	}
}
