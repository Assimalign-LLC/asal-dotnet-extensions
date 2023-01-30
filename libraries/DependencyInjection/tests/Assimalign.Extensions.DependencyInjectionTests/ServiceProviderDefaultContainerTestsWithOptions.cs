using System;
using Assimalign.Extensions.DependencyInjection.Specification;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderDefaultContainerTestsWithOptions : ServiceProviderIoCSpecificationTests
{
    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder) =>
        builder.BuildServiceProvider(ServiceProviderEngine.Default, new()
        {
            ValidateOnBuild = true
        });

    protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder, ServiceProviderOptions options) =>
        builder.BuildServiceProvider(ServiceProviderEngine.Default, options);
    //protected override IServiceProvider CreateServiceProvider(IServiceProviderBuilder collection)
    //{
    //    try
    //    {
    //        return collection.BuildServiceProvider(ServiceProviderMode.Default, new ServiceProviderOptions
    //        {
    //            ValidateOnBuild = true,
    //            // Too many tests fail because they try to resolve scoped services from the root
    //            // provider
    //            // ValidateScopes = true
    //        });
    //    }
    //    catch (AggregateException)
    //    {
    //        // This is how we "skip" tests that fail on BuildServiceProvider (broken object graphs).
    //        // We care mainly about exercising the non-throwing code path so we fallback to the default BuildServiceProvider
    //        return collection.BuildServiceProvider();
    //    }
    
}
