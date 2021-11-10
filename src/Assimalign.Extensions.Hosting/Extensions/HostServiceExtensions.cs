using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Hosting
{
    using Assimalign.Extensions.Hosting.Abstractions;
    using Assimalign.Extensions.DependencyInjection;
    using Assimalign.Extensions.DependencyInjection.Abstractions;

    public static class HostServiceExtensions
    {
        /// <summary>
        /// Add an <see cref="IHostedService"/> registration for the given type.
        /// </summary>
        /// <typeparam name="THostedService">An <see cref="IHostedService"/> to register.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services)
            where THostedService : class, IHostService
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostService, THostedService>());

            return services;
        }

        /// <summary>
        /// Add an <see cref="IHostedService"/> registration for the given type.
        /// </summary>
        /// <typeparam name="THostedService">An <see cref="IHostedService"/> to register.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to register with.</param>
        /// <param name="implementationFactory">A factory to create new instances of the service implementation.</param>
        /// <returns>The original <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHostedService<THostedService>(this IServiceCollection services, Func<IServiceProvider, THostedService> implementationFactory)
            where THostedService : class, IHostService
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostService>(implementationFactory));

            return services;
        }
    }
}
