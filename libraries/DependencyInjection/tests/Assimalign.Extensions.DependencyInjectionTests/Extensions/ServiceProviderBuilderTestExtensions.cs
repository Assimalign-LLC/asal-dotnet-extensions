using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

internal static class ServiceProviderBuilderTestExtensions
{
    public static ServiceProvider BuildServiceProvider(this IServiceProviderBuilder builder, ServiceProviderEngine mode, ServiceProviderOptions options = null)
    {
        if (mode == ServiceProviderEngine.Default)
        {
            return (ServiceProvider)builder.Build();
        }

        var provider = new ServiceProvider(builder.Services, options ?? ServiceProviderOptions.Default);

        provider.engine = mode switch
        {
            ServiceProviderEngine.Dynamic => new DynamicServiceProviderEngine(provider),
            ServiceProviderEngine.Runtime => RuntimeServiceProviderEngine.Instance,
            ServiceProviderEngine.Expressions => new ExpressionsServiceProviderEngine(provider),
            ServiceProviderEngine.ILEmit => new ILEmitServiceProviderEngine(provider),
            _ => throw new NotSupportedException()
        };

        return provider;
    }
}
