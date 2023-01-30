


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderExtensionsTest
{
    [Fact]
    public void GetService_Returns_CorrectService()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(1);

        // Act
        var service = serviceProvider.GetService<IFoo>();

        // Assert
        Assert.IsType<Foo1>(service);
    }

    [Fact]
    public void ISupportRequiredService_GetRequiredService_Returns_CorrectService()
    {
        // Arrange
        var serviceProvider = new RequiredServiceSupportingProvider();

        // Act
        var service = serviceProvider.GetRequiredService<IBar>();

        // Assert
        Assert.IsType<Bar1>(service);
    }

    [Fact]
    public void GetRequiredService_Throws_WhenNoServiceRegistered()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(0);

        // Act + Assert
        AssertExtensions.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IFoo>(),
            $"No service for type '{typeof(IFoo)}' has been registered.");
    }

    [Fact]
    public void ISupportRequiredService_GetRequiredService_Throws_WhenNoServiceRegistered()
    {
        // Arrange
        var serviceProvider = new RequiredServiceSupportingProvider();

        // Act + Assert
        AssertExtensions.Throws<RankException>(() => serviceProvider.GetRequiredService<IFoo>());
    }

    [Fact]
    public void NonGeneric_GetRequiredService_Throws_WhenNoServiceRegistered()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(0);

        // Act + Assert
        AssertExtensions.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService(typeof(IFoo)),
            $"No service for type '{typeof(IFoo)}' has been registered.");
    }

    [Fact]
    public void ISupportRequiredService_NonGeneric_GetRequiredService_Throws_WhenNoServiceRegistered()
    {
        // Arrange
        var serviceProvider = new RequiredServiceSupportingProvider();

        // Act + Assert
        AssertExtensions.Throws<RankException>(() => serviceProvider.GetRequiredService(typeof(IFoo)));
    }

    [Fact]
    public void GetServices_Returns_AllServices()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(2);

        // Act
        var services = serviceProvider.GetServices<IFoo>();

        // Assert
        Assert.Contains(services, item => item is Foo1);
        Assert.Contains(services, item => item is Foo2);
        Assert.Equal(2, services.Count());
    }

    [Fact]
    public void NonGeneric_GetServices_Returns_AllServices()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(2);

        // Act
        var services = serviceProvider.GetServices(typeof(IFoo));

        // Assert
        Assert.Contains(services, item => item is Foo1);
        Assert.Contains(services, item => item is Foo2);
        Assert.Equal(2, services.Count());
    }

    [Fact]
    public void GetServices_Returns_SingleService()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(1);

        // Act
        var services = serviceProvider.GetServices<IFoo>();

        // Assert
        var item = Assert.Single(services);
        Assert.IsType<Foo1>(item);
    }

    [Fact]
    public void NonGeneric_GetServices_Returns_SingleService()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(1);

        // Act
        var services = serviceProvider.GetServices(typeof(IFoo));

        // Assert
        var item = Assert.Single(services);
        Assert.IsType<Foo1>(item);
    }

    [Fact]
    public void GetServices_Returns_CorrectTypes()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(4);

        // Act
        var services = serviceProvider.GetServices(typeof(IBar));

        // Assert
        foreach (var service in services)
        {
            Assert.IsAssignableFrom<IBar>(service);
        }
        Assert.Equal(2, services.Count());
    }

    [Fact]
    public void GetServices_Returns_EmptyArray_WhenNoServicesAvailable()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(0);

        // Act
        var services = serviceProvider.GetServices<IFoo>();

        // Assert
        Assert.Empty(services);
        Assert.IsType<IFoo[]>(services);
    }

    [Fact]
    public void NonGeneric_GetServices_Returns_EmptyArray_WhenNoServicesAvailable()
    {
        // Arrange
        var serviceProvider = CreateTestServiceProvider(0);

        // Act
        var services = serviceProvider.GetServices(typeof(IFoo));

        // Assert
        Assert.Empty(services);
        Assert.IsType<IFoo[]>(services);
    }

    [Fact]
    public void GetServices_WithBuildServiceProvider_Returns_EmptyList_WhenNoServicesAvailable()
    {
        // Arrange
        var builder = new ServiceProviderBuilder();
        builder.AddSingleton<IEnumerable<IFoo>>(new List<IFoo>());
        var serviceProvider = builder.Build();

        // Act
        var services = serviceProvider.GetServices<IFoo>();

        // Assert
        Assert.Empty(services);
        Assert.IsType<List<IFoo>>(services);
    }

    [Fact]
    public void NonGeneric_GetServices_WithBuildServiceProvider_Returns_EmptyList_WhenNoServicesAvailable()
    {
        // Arrange
        var builder = new ServiceProviderBuilder();
        builder.AddSingleton<IEnumerable<IFoo>>(new List<IFoo>());
        var serviceProvider = builder.Build();

        // Act
        var services = serviceProvider.GetServices(typeof(IFoo));

        // Assert
        Assert.Empty(services);
        Assert.IsType<List<IFoo>>(services);
    }

    [Fact]
    public async Task CreateAsyncScope_Returns_AsyncServiceScope_Wrapping_ServiceScope()
    {
        // Arrange
        var builder = new ServiceProviderBuilder();
        builder.AddScoped<IFoo, Foo1>();
        var serviceProvider = builder.Build();

        await using var scope = serviceProvider.CreateAsyncScope();

        // Act
        var service = scope.ServiceProvider.GetService<IFoo>();

        // Assert
        Assert.IsType<Foo1>(service);
    }

    [Fact]
    public async Task CreateAsyncScope_Returns_AsyncServiceScope_Wrapping_ServiceScope_For_IServiceScopeFactory()
    {
        // Arrange
        var builder = new ServiceProviderBuilder();
        builder.AddScoped<IFoo, Foo1>();
        var serviceProvider = builder.Build();
        var factory = serviceProvider.GetService<IServiceScopeFactory>();

        await using var scope = factory.CreateAsyncScope();

        // Act
        var service = scope.ServiceProvider.GetService<IFoo>();

        // Assert
        Assert.IsType<Foo1>(service);
    }

    private static IServiceProvider CreateTestServiceProvider(int count)
    {
        var builder = new ServiceProviderBuilder();

        if (count > 0)
        {
            builder.AddTransient<IFoo, Foo1>();
        }

        if (count > 1)
        {
            builder.AddTransient<IFoo, Foo2>();
        }

        if (count > 2)
        {
            builder.AddTransient<IBar, Bar1>();
        }

        if (count > 3)
        {
            builder.AddTransient<IBar, Bar2>();
        }

        return builder.Build();
    }

    public interface IFoo { }

    public class Foo1 : IFoo { }

    public class Foo2 : IFoo { }

    public interface IBar { }

    public class Bar1 : IBar { }

    public class Bar2 : IBar { }

    private class RequiredServiceSupportingProvider : IServiceProvider, ISupportRequiredService
    {
        object ISupportRequiredService.GetRequiredService(Type serviceType)
        {
            if (serviceType == typeof(IBar))
            {
                return new Bar1();
            }

            throw new RankException();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotSupportedException();
        }
    }
}
