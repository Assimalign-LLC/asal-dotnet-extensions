


using System;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderDynamicContainerTests : ServiceProviderContainerTests
{
    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder collection) =>
        collection.Build();

    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder, ServiceProviderOptions options)
    {
        return new ServiceProvider(builder.Services, options);
    }
}
