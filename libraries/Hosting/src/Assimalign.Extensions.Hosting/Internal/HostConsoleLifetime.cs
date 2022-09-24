
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
    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Hosting;


    /// <summary>
    /// Listens for Ctrl+C or SIGTERM and initiates shutdown.
    /// </summary>
    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public class HostConsoleLifetime : IHostLifetime, IDisposable
    {
		private readonly ManualResetEvent signal = new ManualResetEvent(initialState: false);
		private CancellationTokenRegistration applicationStartedRegistration;
		private CancellationTokenRegistration applicationStoppingRegistration;
		private HostConsoleLifetimeOptions lifetimOptions;
		private IHostEnvironment environment;
		private IHostApplicationLifetime applicationLifetime;
		private HostOptions hostOptions;
		private ILogger logger;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="lifetimOptions"></param>
		/// <param name="environment"></param>
		/// <param name="applicationLifetime"></param>
		/// <param name="hostOptions"></param>
		public HostConsoleLifetime(
			IOptions<HostConsoleLifetimeOptions> lifetimOptions, 
			IHostEnvironment environment, 
			IHostApplicationLifetime applicationLifetime, 
			IOptions<HostOptions> hostOptions)
			: this(lifetimOptions, environment, applicationLifetime, hostOptions, NullLoggerFactory.Instance)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lifetimOptions"></param>
		/// <param name="environment"></param>
		/// <param name="applicationLifetime"></param>
		/// <param name="hostOptions"></param>
		/// <param name="loggerFactory"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public HostConsoleLifetime(
			IOptions<HostConsoleLifetimeOptions> lifetimOptions, 
			IHostEnvironment environment, 
			IHostApplicationLifetime applicationLifetime, 
			IOptions<HostOptions> hostOptions, 
			ILoggerFactory loggerFactory)
		{
			this.lifetimOptions = lifetimOptions.Value ?? throw new ArgumentNullException("options");
			this.environment = environment ?? throw new ArgumentNullException("environment");
			this.applicationLifetime = applicationLifetime ?? throw new ArgumentNullException("applicationLifetime");
			this.hostOptions = hostOptions?.Value ?? throw new ArgumentNullException("hostOptions");
			this.logger = loggerFactory.Create("Assimalign.Hosting.Lifetime");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task WaitForStartAsync(CancellationToken cancellationToken)
		{
			if (!lifetimOptions.SuppressStatusMessages)
			{
				applicationStartedRegistration = applicationLifetime.ApplicationStarted.Register(delegate (object state)
				{
					((HostConsoleLifetime)state).OnApplicationStarted();
				}, this);
				applicationStoppingRegistration = applicationLifetime.ApplicationStopping.Register(delegate (object state)
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
			logger.LogInformation("Application started. Press Ctrl+C to shut down.");
			logger.LogInformation("Hosting environment: {envName}", this.environment.EnvironmentName);
			logger.LogInformation("Content root path: {contentRoot}", this.environment.ContentRootPath);
		}

		private void OnApplicationStopping()
		{
			logger.LogInformation("Application is shutting down...");
		}

		private void OnProcessExit(object sender, EventArgs e)
		{
			applicationLifetime.StopApplication();
			if (!signal.WaitOne(hostOptions.ShutdownTimeout))
			{
				logger.LogInformation("Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.");
			}
			signal.WaitOne();
			System.Environment.ExitCode = 0;
		}

		private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs eventArgs)
		{
			eventArgs.Cancel = true;
			applicationLifetime.StopApplication();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			signal.Set();
			AppDomain.CurrentDomain.ProcessExit -= new EventHandler(OnProcessExit);
			Console.CancelKeyPress -= OnCancelKeyPress;
			applicationStartedRegistration.Dispose();
			applicationStoppingRegistration.Dispose();
		}
	}
}
