using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Assimalign.Extensions.Options
{
    using Assimalign.Extensions.Configuration.Abstractions;
    using Assimalign.Extensions.Configuration.Binder;
    using Assimalign.Extensions.Options.Abstractions;
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.DependencyInjection.Abstractions;

    /// <summary>
    /// Extension methods for adding configuration related options services to the DI container.
    /// </summary>
    public static class OptionsConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        [RequiresUnreferencedCode(OptionsBuilderConfigurationExtensions.TrimmingRequiredUnreferencedCodeMessage)]
        public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, IConfiguration config) where TOptions : class
            => services.Configure<TOptions>(Options.Options<TOptions>.DefaultName, config);

        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        [RequiresUnreferencedCode(OptionsBuilderConfigurationExtensions.TrimmingRequiredUnreferencedCodeMessage)]
        public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, string name, IConfiguration config) where TOptions : class
            => services.Configure<TOptions>(name, config, _ => { });

        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <param name="configureBinder">Used to configure the <see cref="BinderOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        [RequiresUnreferencedCode(OptionsBuilderConfigurationExtensions.TrimmingRequiredUnreferencedCodeMessage)]
        public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, IConfiguration config, Action<ConfigurationBinderOptions> configureBinder)
            where TOptions : class
            => services.Configure<TOptions>(Options.Options<TOptions>.DefaultName, config, configureBinder);

        /// <summary>
        /// Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <param name="configureBinder">Used to configure the <see cref="BinderOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        [RequiresUnreferencedCode(OptionsBuilderConfigurationExtensions.TrimmingRequiredUnreferencedCodeMessage)]
        public static IServiceCollection Configure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this IServiceCollection services, string name, IConfiguration config, Action<ConfigurationBinderOptions> configureBinder)
            where TOptions : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddOptions();
            services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            return services.AddSingleton<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, config, configureBinder));
        }
    }
}
