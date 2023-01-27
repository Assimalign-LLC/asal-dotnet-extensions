


using System;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class ServiceProviderDefaultContainerTests : ServiceProviderContainerTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection collection) =>
            collection.BuildServiceProvider(ServiceProviderMode.Default);
    }
}
