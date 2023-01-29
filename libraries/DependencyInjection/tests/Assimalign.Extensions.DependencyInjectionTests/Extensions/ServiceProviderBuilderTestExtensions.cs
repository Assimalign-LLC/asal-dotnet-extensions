using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.Tests;

internal static class ServiceProviderBuilderTestExtensions
{
    public static ServiceProvider BuildServiceProvider(this IServiceProviderBuilder builder, ServiceProviderMode mode, ServiceProviderOptions options = null)
    {
        if (mode == ServiceProviderMode.Default)
        {
            return (ServiceProvider)builder.Build();
        }

        var provider = new ServiceProvider(builder.Services, options ?? ServiceProviderOptions.Default);

        provider.engine = mode switch
        {
            ServiceProviderMode.Dynamic => new DynamicServiceProviderEngine(provider),
            ServiceProviderMode.Runtime => RuntimeServiceProviderEngine.Instance,
            ServiceProviderMode.Expressions => new ExpressionsServiceProviderEngine(provider),
            ServiceProviderMode.ILEmit => new ILEmitServiceProviderEngine(provider),
            _ => throw new NotSupportedException()
        };

        return provider;
    }
}
