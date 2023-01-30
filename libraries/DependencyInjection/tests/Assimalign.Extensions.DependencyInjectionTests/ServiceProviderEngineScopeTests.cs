


using System;
using Assimalign.Extensions.DependencyInjection.MockObjects;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderEngineScopeTests
{
    [Fact]
    public void DoubleDisposeWorks()
    {
        var provider = new ServiceProvider(new ServiceCollection(), ServiceProviderOptions.Default);
        var serviceProviderEngineScope = new ServiceProviderEngineScope(provider, isRootScope: true);
        serviceProviderEngineScope.ResolvedServices.Add(new CallSiteServiceCacheKey(typeof(IFakeService), 0), null);
        serviceProviderEngineScope.Dispose();
        serviceProviderEngineScope.Dispose();
    }

    [Fact]
    public void RootEngineScopeDisposeTest()
    {
        var services = new ServiceProviderBuilder();
        var sp = services.Build();
        var s = sp.GetRequiredService<IServiceProvider>();
        ((IDisposable)s).Dispose();

        Assert.Throws<ObjectDisposedException>(() => sp.GetRequiredService<IServiceProvider>());
    }
}
