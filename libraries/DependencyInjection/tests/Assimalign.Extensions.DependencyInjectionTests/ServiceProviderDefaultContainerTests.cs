


using System;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class ServiceProviderDefaultContainerTests : ServiceProviderContainerTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder) =>
            builder.BuildServiceProvider(ServiceProviderMode.Default);


        protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder, ServiceProviderOptions options)
        {
            return builder.BuildServiceProvider(ServiceProviderMode.Default, options);
        }
    }
}
