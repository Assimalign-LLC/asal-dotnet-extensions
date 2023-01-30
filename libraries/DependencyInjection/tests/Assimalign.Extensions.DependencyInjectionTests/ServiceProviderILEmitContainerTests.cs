


using System;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderILEmitContainerTests : ServiceProviderContainerTests
{
    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder collection) =>
    collection.BuildServiceProvider(ServiceProviderEngine.ILEmit);

    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder, ServiceProviderOptions options) =>
        builder.BuildServiceProvider(ServiceProviderEngine.ILEmit, options);
}
