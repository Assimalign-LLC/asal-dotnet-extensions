


using System;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderExpressionsContainerTests : ServiceProviderContainerTests
{
    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder collection) =>
        collection.BuildServiceProvider(ServiceProviderEngine.Expressions);

    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder, ServiceProviderOptions options) =>
        builder.BuildServiceProvider(ServiceProviderEngine.Expressions, options);
}
