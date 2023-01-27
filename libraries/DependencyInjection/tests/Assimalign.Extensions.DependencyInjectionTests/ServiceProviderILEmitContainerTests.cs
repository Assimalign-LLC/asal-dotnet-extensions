


using System;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class ServiceProviderILEmitContainerTests : ServiceProviderContainerTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection collection) =>
            collection.BuildServiceProvider(ServiceProviderMode.ILEmit);
    }
}
