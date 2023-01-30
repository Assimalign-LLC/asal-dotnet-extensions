


using System;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderDefaultContainerTests : ServiceProviderContainerTests
{
    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder) =>
        builder.BuildServiceProvider(ServiceProviderEngine.Default);

    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder, ServiceProviderOptions options)=>
        builder.BuildServiceProvider(ServiceProviderEngine.Default, options);
}
