


using System;
using Assimalign.Extensions.DependencyInjection.ServiceLookup;
using Assimalign.Extensions.DependencyInjection.ServiceLookup.ILEmit;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    internal static class ServiceCollectionContainerBuilderTestExtensions
    {
        public static ServiceProvider BuildServiceProvider(this IServiceCollection services, ServiceProviderMode mode, ServiceProviderOptions options = null)
        {
            options ??= ServiceProviderOptions.Default;

            if (mode == ServiceProviderMode.Default)
            {
                return services.BuildServiceProvider(options);
            }

            var provider = new ServiceProvider(services, ServiceProviderOptions.Default);
            ServiceProviderEngine engine = mode switch
            {
                ServiceProviderMode.Dynamic => new DynamicServiceProviderEngine(provider),
                ServiceProviderMode.Runtime => RuntimeServiceProviderEngine.Instance,
                ServiceProviderMode.Expressions => new ExpressionsServiceProviderEngine(provider),
                ServiceProviderMode.ILEmit => new ILEmitServiceProviderEngine(provider),
                _ => throw new NotSupportedException()
            };
            provider._engine = engine;
            return provider;
        }
    }
}
