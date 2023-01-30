using Assimalign.Extensions.DependencyInjection.MockObjects;
using System;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection;

public class ServiceProviderValidationTests
{
 

    [Fact]
    public void GetService_Throws_WhenScopedIsInjectedIntoSingleton()
    {
        // Arrange
        var provider = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateScopes = true
        })
        .AddSingleton<IFoo, Foo>()
        .AddScoped<IBar, Bar>()
        .Build();

        // Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => provider.GetService(typeof(IFoo)));
        Assert.Equal($"Cannot consume scoped service '{typeof(IBar)}' from singleton '{typeof(IFoo)}'.", exception.Message);
    }
    [Fact]
    public void GetService_Throws_WhenScopedIsInjectedIntoSingletonThroughTransient()
    {
        var factory = new ServiceProviderFactory();

        // Arrange
        var provider = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateScopes = true
        })
            .AddSingleton<IFoo, Foo>()
            .AddTransient<IBar, Bar2>()
            .AddScoped<IBaz, Baz>()
            .Build();


        // Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => provider.GetService(typeof(IFoo)));
        Assert.Equal($"Cannot consume scoped service '{typeof(IBaz)}' from singleton '{typeof(IFoo)}'.", exception.Message);
    }

    [Fact]
    public void GetService_Throws_WhenScopedIsInjectedIntoSingletonThroughSingleton()
    {
        // Arrange
        var provider = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateScopes = true
        })
            .AddSingleton<IFoo, Foo>()
            .AddSingleton<IBar, Bar2>()
            .AddScoped<IBaz, Baz>()
            .Build();

        // Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => provider.GetService(typeof(IFoo)));
        Assert.Equal($"Cannot consume scoped service '{typeof(IBaz)}' from singleton '{typeof(IBar)}'.", exception.Message);
    }

    [Fact]
    public void GetService_Throws_WhenScopedIsInjectedIntoSingletonThroughSingletonAndScopedWhileInScope()
    {
        // Arrange
        var provider = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateScopes = true
        })
            .AddScoped<IFoo, Foo>()
            .AddSingleton<IBar, Bar2>()
            .AddScoped<IBaz, Baz>()
            .Build();
        var scope = provider.CreateScope();

        // Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => scope.ServiceProvider.GetService(typeof(IFoo)));
        Assert.Equal($"Cannot consume scoped service '{typeof(IBaz)}' from singleton '{typeof(IBar)}'.", exception.Message);
    }

    [Fact]
    public void GetService_Throws_WhenGetServiceForScopedServiceIsCalledOnRoot()
    {
        // Arrange
        var provider = new ServiceProviderBuilder(new()
        {
            ValidateScopes = true
        })
            .AddScoped<IBaz, Baz>()
            .Build();
        
        // Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => provider.GetService(typeof(IBaz)));
        Assert.Equal($"Cannot resolve scoped service '{typeof(IBaz)}' from root provider.", exception.Message);
    }

    [Fact]
    public void GetService_Throws_WhenGetServiceForScopedServiceIsCalledOnRootViaTransient()
    {
        // Arrange
        var providerBuilder = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateScopes = true
        })
            .AddTransient<IFoo, Foo>()
            .AddScoped<IBar, Bar>();
        var provider = providerBuilder.Build();

        // Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => provider.GetService(typeof(IFoo)));
        Assert.Equal($"Cannot resolve '{typeof(IFoo)}' from root provider because it requires scoped service '{typeof(IBar)}'.", exception.Message);
    }

    [Fact]
    public void GetService_DoesNotThrow_WhenScopeFactoryIsInjectedIntoSingleton()
    {
        // Arrange
        var provider = new ServiceProviderBuilder(new()
        {
            ValidateScopes = true
        })
            .AddSingleton<IBoo, Boo>()
            .Build();


        // Act + Assert
        var result = provider.GetService(typeof(IBoo));
        Assert.NotNull(result);
    }

    [Fact]
    public void BuildServiceProvider_ValidateOnBuild_ThrowsForUnresolvableServices()
    {
        // Arrange
        var builder = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateOnBuild = true
        })
        .AddTransient<IFoo, Foo>()
        .AddTransient<IBaz, BazRecursive>();

        // Act + Assert
        var aggregateException = Assert.Throws<AggregateException>(() => builder.Build());
        Assert.StartsWith("Some services are not able to be constructed", aggregateException.Message);
        Assert.Equal(2, aggregateException.InnerExceptions.Count);
        Assert.Equal("Error while validating the service descriptor 'ServiceType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IFoo Lifetime: Transient ImplementationType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+Foo': " +
                     "Unable to resolve service for type 'Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBar' while attempting to activate" +
                     " 'Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+Foo'.",
            aggregateException.InnerExceptions[0].Message);

        Assert.Equal("Error while validating the service descriptor 'ServiceType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz Lifetime: Transient ImplementationType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+BazRecursive': " +
                     "A circular dependency was detected for the service of type 'Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz'." + Environment.NewLine +
                     "Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz(Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+BazRecursive) ->" +
                     " Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz",
            aggregateException.InnerExceptions[1].Message);
    }

    [Fact]
    public void BuildServiceProvider_ValidateOnBuild_SkipsOpenGenerics()
    {
        // Arrange
        var builder = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateOnBuild = true
        })
        .AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>))
        .Build();

    }

    [Fact]
    public void BuildServiceProvider_ValidateOnBuild_ValidatesAllDescriptors()
    {
        // Arrange
        var builder = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateOnBuild = true
        })
        .AddTransient<IBaz, BazRecursive>()
        .AddTransient<IBaz, Baz>();

        // Act + Assert
        var aggregateException = Assert.Throws<AggregateException>(() => builder.Build());
        Assert.StartsWith("Some services are not able to be constructed", aggregateException.Message);
        Assert.Single(aggregateException.InnerExceptions);

        Assert.Equal("Error while validating the service descriptor 'ServiceType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz Lifetime: Transient ImplementationType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+BazRecursive': " +
                     "A circular dependency was detected for the service of type 'Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz'." + Environment.NewLine +
                     "Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz(Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+BazRecursive) ->" +
                     " Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz",
            aggregateException.InnerExceptions[0].Message);
    }

    [Fact]
    public void BuildServiceProvider_ValidateOnBuild_ThrowsWhenImplementationIsNotAssignableToService()
    {
        // Arrange
        var builder = new ServiceProviderBuilder(new ServiceProviderOptions()
        {
            ValidateOnBuild = true
        })
            .AddTransient(typeof(IBaz), typeof(Boo))
            .AddSingleton(typeof(IFoo), new object());


        // Act + Assert
        var aggregateException = Assert.Throws<AggregateException>(() => builder.Build());
        Assert.StartsWith("Some services are not able to be constructed", aggregateException.Message);
        Assert.Equal(2, aggregateException.InnerExceptions.Count);

        Assert.Equal("Error while validating the service descriptor 'ServiceType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz Lifetime: Transient ImplementationType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+Boo': " +
                     "Implementation type 'Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+Boo' can't be converted to service type 'Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IBaz'",
                     aggregateException.InnerExceptions[0].Message);

        Assert.Equal("Error while validating the service descriptor 'ServiceType: Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IFoo Lifetime: Singleton ImplementationInstance: System.Object': " +
                     "Constant value of type 'System.Object' can't be converted to service type 'Assimalign.Extensions.DependencyInjection.ServiceProviderValidationTests+IFoo'",
                     aggregateException.InnerExceptions[1].Message);
    }

    private interface IFoo
    {
    }

    private class Foo : IFoo
    {
        public Foo(IBar bar)
        {
        }
    }

    private interface IBar
    {
    }

    private class Bar : IBar
    {
    }

    private class Bar2 : IBar
    {
        public Bar2(IBaz baz)
        {
        }
    }

    private interface IBaz
    {
    }

    private class Baz : IBaz
    {
    }

    private class BazRecursive : IBaz
    {
        public BazRecursive(IBaz baz)
        {
        }
    }

    private interface IBoo
    {
    }

    private class Boo : IBoo
    {
        public Boo(IServiceScopeFactory scopeFactory)
        {
        }
    }
}
