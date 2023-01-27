


using System;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class ServiceProviderDynamicContainerTests : ServiceProviderContainerTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection collection) =>
            collection.BuildServiceProvider();
    }
}
