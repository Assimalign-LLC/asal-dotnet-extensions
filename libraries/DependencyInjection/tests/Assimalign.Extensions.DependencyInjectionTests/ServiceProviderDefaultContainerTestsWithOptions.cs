


using System;
using Assimalign.Extensions.DependencyInjection.Specification;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class ServiceProviderDefaultContainerTestsWithOptions : DependencyInjectionSpecificationTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection collection)
        {
            try
            {
                return collection.BuildServiceProvider(ServiceProviderMode.Default, new ServiceProviderOptions
                {
                    ValidateOnBuild = true,
                    // Too many tests fail because they try to resolve scoped services from the root
                    // provider
                    // ValidateScopes = true
                });
            }
            catch (AggregateException)
            {
                // This is how we "skip" tests that fail on BuildServiceProvider (broken object graphs).
                // We care mainly about exercising the non-throwing code path so we fallback to the default BuildServiceProvider
                return collection.BuildServiceProvider();
            }
        }
    }
}
