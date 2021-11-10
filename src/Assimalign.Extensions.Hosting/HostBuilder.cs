using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Configuration;
    using Assimalign.Extensions.Configuration.Abstractions;
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.DependencyInjection.Abstractions;
    using Assimalign.Extensions.FileProviders.Physical;
    using Assimalign.Extensions.Hosting.Internal;
    using Assimalign.Extensions.Hosting.Abstractions;
    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Logging.Abstractions;
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Options.Abstractions;




    /// <summary>
    /// A program initialization utility.
    /// </summary>
    public partial class HostBuilder : IHostBuilder
    {
        private List<Action<IConfigurationBuilder>> _configureHostConfigActions = new();
        private List<Action<HostBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new();
        private List<Action<HostBuilderContext, IServiceCollection>> _configureServicesActions = new();
        private List<IHostConfigureContainerAdapter> configureContainerActions = new List<IHostConfigureContainerAdapter>();
        
        
        private IHostServiceFactoryAdapter hostServiceProviderFactory = new HostServiceFactoryAdapter<IServiceCollection>(new ServiceProviderFactoryDefault());
        
        private bool isHostBuilt;
        private IConfiguration hostConfiguration;
        private IConfiguration appConfiguration;
        private HostBuilderContext hostBuilderContext;
        private HostEnvironment hostEnvironment;
        private IServiceProvider appServiceProvider;
        private PhysicalFileProvider defaultProvider;

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        /// <summary>
        /// Set up the configuration for the builder itself. This will be used to initialize the <see cref="IHostEnvironment"/>
        /// for use later in the build process. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _configureHostConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Sets up the configuration for the remainder of the build process and application. This can be called multiple times and
        /// the results will be additive. The results will be available at <see cref="HostBuilderContext.Configuration"/> for
        /// subsequent operations, as well as in <see cref="IHost.Services"/>.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _configureAppConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Overrides the factory used to create the service provider.
        /// </summary>
        /// <typeparam name="TContainerBuilder">The type of the builder to create.</typeparam>
        /// <param name="factory">A factory used for creating service providers.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            hostServiceProviderFactory = new HostServiceFactoryAdapter<TContainerBuilder>(factory ?? throw new ArgumentNullException(nameof(factory)));
            return this;
        }

        /// <summary>
        /// Overrides the factory used to create the service provider.
        /// </summary>
        /// <param name="factory">A factory used for creating service providers.</param>
        /// <typeparam name="TContainerBuilder">The type of the builder to create.</typeparam>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            hostServiceProviderFactory = new HostServiceFactoryAdapter<TContainerBuilder>(() => hostBuilderContext, factory ?? throw new ArgumentNullException(nameof(factory)));
            return this;
        }

        /// <summary>
        /// Enables configuring the instantiated dependency container. This can be called multiple times and
        /// the results will be additive.
        /// </summary>
        /// <typeparam name="TContainerBuilder">The type of the builder to create.</typeparam>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            configureContainerActions.Add(new HostConfigureContainerAdapter<TContainerBuilder>(configureDelegate
                ?? throw new ArgumentNullException(nameof(configureDelegate))));
            return this;
        }

        /// <summary>
        /// Run the given actions to initialize the host. This can only be called once.
        /// </summary>
        /// <returns>An initialized <see cref="IHost"/></returns>
        public IHost Build()
        {
            if (isHostBuilt)
            {
                throw new InvalidOperationException();// SR.BuildCalled);
            }
            isHostBuilt = true;

            // REVIEW: If we want to raise more events outside of these calls then we will need to
            // stash this in a field.
            using var diagnosticListener = new DiagnosticListener("Assimalign.Extensions.Hosting");
            const string hostBuildingEventName = "HostBuilding";
            const string hostBuiltEventName = "HostBuilt";

            if (diagnosticListener.IsEnabled() && diagnosticListener.IsEnabled(hostBuildingEventName))
            {
                Write(diagnosticListener, hostBuildingEventName, this);
            }

            BuildHostConfiguration();
            CreateHostingEnvironment();
            CreateHostBuilderContext();
            BuildAppConfiguration();
            CreateServiceProvider();

            var host = appServiceProvider.GetRequiredService<IHost>();
            if (diagnosticListener.IsEnabled() && diagnosticListener.IsEnabled(hostBuiltEventName))
            {
                Write(diagnosticListener, hostBuiltEventName, host);
            }

            return host;
        }

        //[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:UnrecognizedReflectionPattern",
        //    Justification = "The values being passed into Write are being consumed by the application already.")]
        private static void Write<T>(
            DiagnosticSource diagnosticSource,
            string name,
            T value)
        {
            diagnosticSource.Write(name, value);
        }

        private void BuildHostConfiguration()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(); // Make sure there's some default storage since there are no default providers

            foreach (Action<IConfigurationBuilder> buildAction in _configureHostConfigActions)
            {
                buildAction(configBuilder);
            }
            hostConfiguration = configBuilder.Build();
        }

        private void CreateHostingEnvironment()
        {
            hostEnvironment = new HostEnvironment()
            {
                ApplicationName = hostConfiguration[HostDefaults.ApplicationKey],
                EnvironmentName = hostConfiguration[HostDefaults.EnvironmentKey] ?? Environments.HostEnvironments.Production,
                ContentRootPath = ResolveContentRootPath(hostConfiguration[HostDefaults.ContentRootKey], AppContext.BaseDirectory),
            };

            if (string.IsNullOrEmpty(hostEnvironment.ApplicationName))
            {
                // Note GetEntryAssembly returns null for the net4x console test runner.
                hostEnvironment.ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name;
            }

            hostEnvironment.ContentRootFileProvider = defaultProvider = new PhysicalFileProvider(hostEnvironment.ContentRootPath);
        }

        private string ResolveContentRootPath(string contentRootPath, string basePath)
        {
            if (string.IsNullOrEmpty(contentRootPath))
            {
                return basePath;
            }
            if (Path.IsPathRooted(contentRootPath))
            {
                return contentRootPath;
            }
            return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
        }

        private void CreateHostBuilderContext()
        {
            hostBuilderContext = new HostBuilderContext(Properties)
            {
                HostingEnvironment = hostEnvironment,
                Configuration = hostConfiguration
            };
        }

        private void BuildAppConfiguration()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddConfiguration(hostConfiguration, shouldDisposeConfiguration: true);

            foreach (Action<HostBuilderContext, IConfigurationBuilder> buildAction in _configureAppConfigActions)
            {
                buildAction(hostBuilderContext, configBuilder);
            }
            appConfiguration = configBuilder.Build();
            hostBuilderContext.Configuration = appConfiguration;
        }

        private void CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IHostEnvironment>(hostEnvironment);

            services.AddSingleton<IHostEnvironment>(hostEnvironment);
            services.AddSingleton(hostBuilderContext);
            // register configuration as factory to make it dispose with the service provider
            services.AddSingleton(_ => appConfiguration);

            services.AddSingleton<IHostApplicationLifetime>(s => (IHostApplicationLifetime)s.GetService<IHostApplicationLifetime>());

            services.AddSingleton<IHostApplicationLifetime, HostApplicationLifetime>();

            AddLifetime(services);

            services.AddSingleton<IHost>(_ =>
            {
                return new Internal.Host(appServiceProvider,
                    hostEnvironment,
                    defaultProvider,
                    appServiceProvider.GetRequiredService<IHostApplicationLifetime>(),
                    appServiceProvider.GetRequiredService<ILogger<Internal.Host>>(),
                    appServiceProvider.GetRequiredService<IHostLifetime>(),
                    appServiceProvider.GetRequiredService<IOptions<HostOptions>>());
            });
            services.AddOptions().Configure<HostOptions>(options => { options.Initialize(hostConfiguration); });
            services.AddLogging();

            foreach (Action<HostBuilderContext, IServiceCollection> configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(hostBuilderContext, services);
            }

            object containerBuilder = hostServiceProviderFactory.CreateBuilder(services);

            foreach (IHostConfigureContainerAdapter containerAction in configureContainerActions)
            {
                containerAction.ConfigureContainer(hostBuilderContext, containerBuilder);
            }

            appServiceProvider = hostServiceProviderFactory.CreateServiceProvider(containerBuilder);

            if (appServiceProvider == null)
            {
                throw new InvalidOperationException();// SR.NullIServiceProvider);
            }

            // resolve configuration explicitly once to mark it as resolved within the
            // service provider, ensuring it will be properly disposed with the provider
            _ = appServiceProvider.GetService<IConfiguration>();
        }


        private static void AddLifetime(ServiceCollection services)
        {
            if (!OperatingSystem.IsAndroid() && !OperatingSystem.IsBrowser() && !OperatingSystem.IsIOS() && !OperatingSystem.IsTvOS())
            {
                services.AddSingleton<IHostLifetime, HostConsoleLifetime>();
            }
            else
            {
                services.AddSingleton<IHostLifetime, HostNullLifetime>();
            }
        }
    }
}

