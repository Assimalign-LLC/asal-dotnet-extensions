


using System;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class ServiceProviderExpressionsContainerTests : ServiceProviderContainerTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection collection) =>
            collection.BuildServiceProvider(ServiceProviderMode.Expressions);
    }
}
