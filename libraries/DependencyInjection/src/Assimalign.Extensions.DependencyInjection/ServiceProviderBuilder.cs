using System;

namespace Assimalign.Extensions.DependencyInjection;

public sealed class ServiceProviderBuilder : IServiceProviderBuilder
{
    private readonly ServiceProviderOptions options;
    public ServiceProviderBuilder()
    {
        this.options = ServiceProviderOptions.Default;
    }
    public ServiceProviderBuilder(ServiceProviderOptions options)
    {
        this.options = options;
    }

    private readonly IServiceCollection services = new ServiceCollection();
    public IServiceCollection Services => this.services;
    public IServiceProviderBuilder Add(ServiceDescriptor serviceDescriptor)
    {
        if (serviceDescriptor == null)
        {
            throw new ArgumentNullException(nameof(serviceDescriptor));
        }

        Services.Add(serviceDescriptor);

        return this;
    }
    public IServiceProvider Build()
    {
        return new ServiceProvider(
            Services, 
            options);
    }
}
