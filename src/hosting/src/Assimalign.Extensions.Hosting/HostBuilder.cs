using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Configuration;
    using Assimalign.Extensions.Configuration;
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.FileProviders.Physical;
    using Assimalign.Extensions.Hosting.Internal;
    using Assimalign.Extensions.Hosting;
    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Logging;
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.Options;
    using Assimalign.Extensions.FileProviders;


    /// <summary>
    /// A program initialization utility.
    /// </summary>
    public sealed class HostBuilder : IHostBuilder
    {
        private bool isHostBuilt;

        private IList<Action<IConfigurationBuilder>> hostConfigurationActions;
        private IList<Action<HostBuilderContext, IServiceCollection>> hostServiceCollectionActions;
        private IList<Action<HostBuilderContext, IConfigurationBuilder>> appConfigurationActions;
        private IList<IHostConfigureContainerAdapter> containerConfigurationActions;
        private IServiceProvider appServiceProvider;
        private IConfiguration appConfiguration;
        private IHostServiceFactoryAdapter hostServiceProviderFactory;
        private IHostEnvironment hostEnvironment;
        private IConfiguration hostConfiguration;
        private HostBuilderContext hostBuilderContext;
        private IFileProvider hostDefaultFileProvider;
        private Func<IServiceProvider, IHost> implementation;

        internal HostBuilder(Func<IServiceProvider, IHost> implementation) : this()
        {
            this.implementation = implementation;
        }

        internal HostBuilder() 
        {
            this.hostConfigurationActions = new List<Action<IConfigurationBuilder>>();
            this.hostServiceCollectionActions = new List<Action<HostBuilderContext, IServiceCollection>>();
            this.hostServiceProviderFactory = new HostServiceFactoryAdapter<IServiceCollection>(new ServiceProviderFactoryDefault());
            this.appConfigurationActions = new List<Action<HostBuilderContext, IConfigurationBuilder>>();
            this.containerConfigurationActions = new List<IHostConfigureContainerAdapter>();
        }


        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        /// <summary>
        /// Enables configuring the instantiated dependency container. This can be called multiple times and
        /// the results will be additive.
        /// </summary>
        /// <typeparam name="TContainerBuilder">The type of the builder to create.</typeparam>
        /// <param name="configure">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configure)
        {
            containerConfigurationActions.Add(new HostConfigureContainerAdapter<TContainerBuilder>(configure
                ?? throw new ArgumentNullException(nameof(configure))));

            return this;
        }

        /// <summary>
        /// Set up the configuration for the builder itself. This will be used to initialize the <see cref="IHostEnvironment"/>
        /// for use later in the build process. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configure">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configure)
        {
            hostConfigurationActions.Add(configure ?? 
                throw new ArgumentNullException(nameof(configure)));

            return this;
        }

        /// <summary>
        /// Sets up the configuration for the remainder of the build process and application. This can be called multiple times and
        /// the results will be additive. The results will be available at <see cref="HostBuilderContext.Configuration"/> for
        /// subsequent operations, as well as in <see cref="IHost.Services"/>.
        /// </summary>
        /// <param name="configure">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configure)
        {
            appConfigurationActions.Add(configure ?? 
                throw new ArgumentNullException(nameof(configure)));

            return this;
        }

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configure">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configure)
        {
            hostServiceCollectionActions.Add(configure ?? 
                throw new ArgumentNullException(nameof(configure)));

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
            hostServiceProviderFactory = new HostServiceFactoryAdapter<TContainerBuilder>(factory ??
                throw new ArgumentNullException(nameof(factory)));

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
            hostServiceProviderFactory = new HostServiceFactoryAdapter<TContainerBuilder>(() => hostBuilderContext, factory ?? 
                throw new ArgumentNullException(nameof(factory)));

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
                throw new InvalidOperationException("Build has already been called.");// SR.BuildCalled);
            }
            else
            {
                isHostBuilt = true;

                // REVIEW: If we want to raise more events outside of these calls then we will need to
                // stash this in a field.
                using (var diagnosticListener = new DiagnosticListener("Assimalign.Extensions.Hosting"))
                {
                    const string hostBuildingEventName = "HostBuilding";
                    const string hostBuiltEventName = "HostBuilt";

                    if (diagnosticListener.IsEnabled() && 
                        diagnosticListener.IsEnabled(hostBuildingEventName))
                    {
                        diagnosticListener.Write(hostBuildingEventName, this);
                    }

                    BuildHostConfiguration();
                    CreateHostEnvironment();
                    CreateHostBuilderContext();
                    BuildAppConfiguration();
                    CreateServiceProvider();

                    var host = appServiceProvider.GetRequiredService<IHost>();

                    if (diagnosticListener.IsEnabled() && 
                        diagnosticListener.IsEnabled(hostBuiltEventName))
                    {
                        diagnosticListener.Write(hostBuiltEventName, host);
                    }

                    return host;
                }
            }
        }

        /// <summary>
        /// Implements the default IHost with Host Builder.
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder Create() =>
            new HostBuilder();

        /// <summary>
        /// Implements the default host builder with Custom Host.
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder CreateWithHost(Func<IServiceProvider, IHost> implementation) =>
            new HostBuilder(implementation);


        private void BuildHostConfiguration()
        { 
            // Make sure there's some default storage since there are no default providers
            var hostConfigurationBuilder = new ConfigurationBuilder().AddInMemoryCollection();

            foreach (var action in hostConfigurationActions)
            {
                action.Invoke(hostConfigurationBuilder);
            }

            hostConfiguration = hostConfigurationBuilder.Build();
        }
        private void CreateHostEnvironment()
        {
            hostEnvironment = new HostEnvironment()
            {
                ApplicationName = hostConfiguration[HostDefaultConfigurationKeys.ApplicationKey],
                EnvironmentName = hostConfiguration[HostDefaultConfigurationKeys.EnvironmentKey] ?? Environments.HostEnvironments.Production,
                ContentRootPath = ResolveContentRootPath(hostConfiguration[HostDefaultConfigurationKeys.ContentRootKey], AppContext.BaseDirectory),
            };

            if (string.IsNullOrEmpty(hostEnvironment.ApplicationName))
            {
                // Note GetEntryAssembly returns null for the net4x console test runner.
                hostEnvironment.ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name;
            }

            hostDefaultFileProvider = new PhysicalFileProvider(hostEnvironment.ContentRootPath);
            hostEnvironment.ContentRootFileProvider = hostDefaultFileProvider;
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
            var appConfigurationBuilder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddConfiguration(hostConfiguration, shouldDisposeConfiguration: true);

            foreach (var action in appConfigurationActions)
            {
                action.Invoke(hostBuilderContext, appConfigurationBuilder);
            }

            appConfiguration = appConfigurationBuilder.Build();
            hostBuilderContext.Configuration = appConfiguration;
        }
        private void CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IHostEnvironment>(hostEnvironment);
            services.AddSingleton<IHostEnvironment>(hostEnvironment);
            services.AddSingleton(hostBuilderContext);
            services.AddSingleton(serviceProvider => appConfiguration); // register configuration as factory to make it dispose with the service provider
            services.AddSingleton<IHostApplicationLifetime>(serviceProvider => serviceProvider.GetService<IHostApplicationLifetime>());
            services.AddSingleton<IHostApplicationLifetime, HostApplicationLifetime>();

            if (!OperatingSystem.IsAndroid() && 
                !OperatingSystem.IsBrowser() && 
                !OperatingSystem.IsIOS() && 
                !OperatingSystem.IsTvOS())
            {
                services.AddSingleton<IHostLifetime, HostConsoleLifetime>();
            }
            else
            {
                services.AddSingleton<IHostLifetime, HostNullLifetime>();
            }

            if (implementation is null)
            {
                services.AddSingleton<IHost>(serviceProvider =>
                {
                    return new HostDefault(
                        appServiceProvider,
                        hostEnvironment,
                        hostDefaultFileProvider,
                        appServiceProvider.GetRequiredService<IHostApplicationLifetime>(),
                        appServiceProvider.GetRequiredService<ILogger<HostDefault>>(),
                        appServiceProvider.GetRequiredService<IHostLifetime>(),
                        appServiceProvider.GetRequiredService<IOptions<HostOptions>>());
                });
            }
            else
            {
                services.AddSingleton<IHost>(implementation);
            }

            services.AddOptions()
                .Configure<HostOptions>(options => 
                { 
                    options.Initialize(hostConfiguration); 
                });

            services.AddLogging();

            foreach (var action in hostServiceCollectionActions)
            {
                action.Invoke(hostBuilderContext, services);
            }

            var containerBuilder = hostServiceProviderFactory.CreateBuilder(services);

            foreach (var action in containerConfigurationActions)
            {
                action.ConfigureContainer(hostBuilderContext, containerBuilder);
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


        
    }
}

