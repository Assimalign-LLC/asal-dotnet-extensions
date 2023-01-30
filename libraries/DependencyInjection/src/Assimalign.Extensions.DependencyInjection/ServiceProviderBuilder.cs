using System;
using System.Collections.Concurrent;

namespace Assimalign.Extensions.DependencyInjection;

public sealed class ServiceProviderBuilder : IServiceProviderBuilder, IDisposable
{
    private static readonly ConcurrentDictionary<int, IServiceCollection> services = new();
    private static readonly ConcurrentDictionary<int, IServiceProvider> providers = new();
    
    
    private readonly ServiceProviderOptions options;
    public ServiceProviderBuilder() => this.options = ServiceProviderOptions.Default;
    public ServiceProviderBuilder(ServiceProviderOptions options) => this.options = options == null ? throw new ArgumentNullException(nameof(options)) : options;

    public IServiceCollection Services => services.GetOrAdd(this.GetHashCode(), new ServiceCollection());    
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

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
