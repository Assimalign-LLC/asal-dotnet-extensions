using System;
using System.Collections;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup;

using Assimalign.Extensions.DependencyInjection.Utilities;
using Assimalign.Extensions.DependencyInjection.MockObjects;

public class CallSiteTests
{
    public static IEnumerable<object[]> TestServiceDescriptors(ServiceLifetime lifetime)
    {
        Func<object, object, bool> compare;

        if (lifetime == ServiceLifetime.Transient)
        {
            // Expect service references to be different for transient builder
            compare = (service1, service2) => service1 != service2;
        }
        else
        {
            // Expect service references to be the same for singleton and scoped builder
            compare = (service1, service2) => service1 == service2;
        }

        // Implementation Type Descriptor
        yield return new object[]
        {
            new[] { new ServiceDescriptor(typeof(IFakeService), typeof(FakeService), lifetime) },
            typeof(IFakeService),
            compare,
        };
        // Closed Generic Descriptor
        yield return new object[]
        {
            new[] { new ServiceDescriptor(typeof(IFakeOpenGenericService<PocoClass>), typeof(FakeService), lifetime) },
            typeof(IFakeOpenGenericService<PocoClass>),
            compare,
        };
        // Open Generic Descriptor
        yield return new object[]
        {
            new[]
            {
                new ServiceDescriptor(typeof(IFakeService), typeof(FakeService), lifetime),
                new ServiceDescriptor(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>), lifetime),
            },
            typeof(IFakeOpenGenericService<IFakeService>),
            compare,
        };
        // Factory Descriptor
        yield return new object[]
        {
            new[] { new ServiceDescriptor(typeof(IFakeService), _ => new FakeService(), lifetime) },
            typeof(IFakeService),
            compare,
        };

        if (lifetime == ServiceLifetime.Singleton)
        {
            // Instance Descriptor
            yield return new object[]
            {
               new[] { new ServiceDescriptor(typeof(IFakeService), new FakeService()) },
               typeof(IFakeService),
               compare,
            };
        }
    }

    [Theory]
    [MemberData(nameof(TestServiceDescriptors), ServiceLifetime.Singleton)]
    [MemberData(nameof(TestServiceDescriptors), ServiceLifetime.Scoped)]
    [MemberData(nameof(TestServiceDescriptors), ServiceLifetime.Transient)]
    public void BuiltExpressionWillReturnResolvedServiceWhenAppropriate(
        ServiceDescriptor[] descriptors, Type serviceType, Func<object, object, bool> compare)
    {
        var provider = new ServiceProvider(descriptors, ServiceProviderOptions.Default);

        var callSite = provider.CallSiteFactory.GetCallSite(serviceType, new CallSiteChain());
        var collectionCallSite = provider.CallSiteFactory.GetCallSite(typeof(IEnumerable<>).MakeGenericType(serviceType), new CallSiteChain());

        var compiledCallSite = CompileCallSite(callSite, provider);
        var compiledCollectionCallSite = CompileCallSite(collectionCallSite, provider);

        using var scope = (ServiceProviderEngineScope)provider.CreateScope();

        var service1 = Invoke(callSite, scope);
        var service2 = compiledCallSite(scope);
        var serviceEnumerator = ((IEnumerable)compiledCollectionCallSite(scope)).GetEnumerator();

        Assert.NotNull(service1);
        Assert.True(compare(service1, service2));

        // Service can be IEnumerable resolved. The IEnumerable should have exactly one element.
        Assert.True(serviceEnumerator.MoveNext());
        Assert.True(compare(service1, serviceEnumerator.Current));
        Assert.False(serviceEnumerator.MoveNext());
    }

    [Fact]
    public void BuiltExpressionCanResolveNestedScopedService()
    {
        var builder = new ServiceProviderBuilder();
        builder.AddScoped<ServiceA>();
        builder.AddScoped<ServiceB>();
        builder.AddScoped<ServiceC>();

        var provider = (ServiceProvider)builder.Build();
        var callSite = provider.CallSiteFactory.GetCallSite(typeof(ServiceC), new CallSiteChain());
        var compiledCallSite = CompileCallSite(callSite, provider);

        using var scope = (ServiceProviderEngineScope)provider.CreateScope();

        var serviceC = (ServiceC)compiledCallSite(scope);

        Assert.NotNull(serviceC.ServiceB.ServiceA);
        Assert.Equal(serviceC, Invoke(callSite, scope));
    }

    [Theory]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    [InlineData(ServiceLifetime.Singleton)]
    public void BuildExpressionAddsDisposableCaptureForDisposableServices(ServiceLifetime lifetime)
    {
        IServiceCollection descriptors = new ServiceCollection();
        descriptors.Add(ServiceDescriptor.Describe(typeof(ServiceA), typeof(DisposableServiceA), lifetime));
        descriptors.Add(ServiceDescriptor.Describe(typeof(ServiceB), typeof(DisposableServiceB), lifetime));
        descriptors.Add(ServiceDescriptor.Describe(typeof(ServiceC), typeof(DisposableServiceC), lifetime));

        var disposables = new List<object>();
        var provider = new ServiceProvider(descriptors, ServiceProviderOptions.Default);

        var callSite = provider.CallSiteFactory.GetCallSite(typeof(ServiceC), new CallSiteChain());
        var compiledCallSite = CompileCallSite(callSite, provider);

        var serviceC = (DisposableServiceC)compiledCallSite(provider.Root);

        Assert.Equal(3, provider.Root.Disposables.Count);
    }

    [Theory]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    [InlineData(ServiceLifetime.Singleton)]
    public void BuildExpressionAddsDisposableCaptureForDisposableFactoryServices(ServiceLifetime lifetime)
    {
        IServiceCollection descriptors = new ServiceCollection();
        descriptors.Add(ServiceDescriptor.Describe(typeof(ServiceA), typeof(DisposableServiceA), lifetime));
        descriptors.Add(ServiceDescriptor.Describe(typeof(ServiceB), typeof(DisposableServiceB), lifetime));
        descriptors.Add(ServiceDescriptor.Describe(
            typeof(ServiceC), p => new DisposableServiceC(p.GetService<ServiceB>()), lifetime));

        var disposables = new List<object>();
        var provider = new ServiceProvider(descriptors, ServiceProviderOptions.Default);

        var callSite = provider.CallSiteFactory.GetCallSite(typeof(ServiceC), new CallSiteChain());
        var compiledCallSite = CompileCallSite(callSite, provider);

        var serviceC = (DisposableServiceC)compiledCallSite(provider.Root);

        Assert.Equal(3, provider.Root.Disposables.Count);
    }

    [Theory]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    // We are not testing singleton here because singleton resolutions always got through
    // runtime resolver and there is no sense to eliminating call from there
    public void BuildExpressionElidesDisposableCaptureForNonDisposableServices(ServiceLifetime lifetime)
    {
        var builder = new ServiceProviderBuilder();
        builder.Add(ServiceDescriptor.Describe(typeof(ServiceA), typeof(ServiceA), lifetime));
        builder.Add(ServiceDescriptor.Describe(typeof(ServiceB), typeof(ServiceB), lifetime));
        builder.Add(ServiceDescriptor.Describe(typeof(ServiceC), typeof(ServiceC), lifetime));

        builder.AddScoped<ServiceB>();
        builder.AddTransient<ServiceC>();

        var disposables = new List<object>();
        var provider = (ServiceProvider)builder.Build();

        var callSite = provider.CallSiteFactory.GetCallSite(typeof(ServiceC), new CallSiteChain());
        var compiledCallSite = CompileCallSite(callSite, provider);

        var serviceC = (ServiceC)compiledCallSite(provider.Root);

        Assert.Empty(provider.Root.Disposables);
    }

    [Theory]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    // We are not testing singleton here because singleton resolutions always got through
    // runtime resolver and there is no sense to eliminating call from there
    public void BuildExpressionElidesDisposableCaptureForEnumerableServices(ServiceLifetime lifetime)
    {
        IServiceCollection descriptors = new ServiceCollection();
        descriptors.Add(ServiceDescriptor.Describe(typeof(ServiceA), typeof(ServiceA), lifetime));
        descriptors.Add(ServiceDescriptor.Describe(typeof(ServiceD), typeof(ServiceD), lifetime));

        var disposables = new List<object>();
        var provider = new ServiceProvider(descriptors, ServiceProviderOptions.Default);

        var callSite = provider.CallSiteFactory.GetCallSite(typeof(ServiceD), new CallSiteChain());
        var compiledCallSite = CompileCallSite(callSite, provider);

        var serviceD = (ServiceD)compiledCallSite(provider.Root);

        Assert.Empty(provider.Root.Disposables);
    }

    [Fact]
    public void BuiltExpressionRethrowsOriginalExceptionFromConstructor()
    {
        var builder = new ServiceProviderBuilder();
        builder.AddTransient<ClassWithThrowingEmptyCtor>();
        builder.AddTransient<ClassWithThrowingCtor>();
        builder.AddTransient<IFakeService, FakeService>();

        var provider = (ServiceProvider)builder.Build();

        var callSite1 = provider.CallSiteFactory.GetCallSite(typeof(ClassWithThrowingEmptyCtor), new CallSiteChain());
        var compiledCallSite1 = CompileCallSite(callSite1, provider);

        var callSite2 = provider.CallSiteFactory.GetCallSite(typeof(ClassWithThrowingCtor), new CallSiteChain());
        var compiledCallSite2 = CompileCallSite(callSite2, provider);

        var ex1 = Assert.Throws<Exception>(() => compiledCallSite1(provider.Root));
        Assert.Equal(nameof(ClassWithThrowingEmptyCtor), ex1.Message);

        var ex2 = Assert.Throws<Exception>(() => compiledCallSite2(provider.Root));
        Assert.Equal(nameof(ClassWithThrowingCtor), ex2.Message);
    }

    [Fact]
    public void DoesNotThrowWhenServiceIsUsedAsEnumerableAndNotInOneCallSite()
    {
        var builder = new ServiceProviderBuilder();
        builder.AddTransient<ServiceA>();
        builder.AddTransient<ServiceD>();
        builder.AddTransient<ServiceE>();

        var provider = (ServiceProvider)builder.Build();

        var callSite1 = provider.CallSiteFactory.GetCallSite(typeof(ServiceE), new CallSiteChain());
        var compileCallSite = CompileCallSite(callSite1, provider);

        Assert.NotNull(compileCallSite);
    }

    [Theory]
    [InlineData(ServiceProviderEngine.Default)]
    [InlineData(ServiceProviderEngine.Dynamic)]
    [InlineData(ServiceProviderEngine.Runtime)]
    [InlineData(ServiceProviderEngine.Expressions)]
    [InlineData(ServiceProviderEngine.ILEmit)]
    private void NoServiceCallsite_DefaultValueNull_DoesNotThrow(ServiceProviderEngine mode)
    {
        var builder = new ServiceProviderBuilder();
        builder.AddTransient<ServiceG>();

        var provider = builder.BuildServiceProvider(mode);
        ServiceF instance = ActivatorUtilities.CreateInstance<ServiceF>(provider);

        Assert.NotNull(instance);
    }

    //[Fact]
    //[ActiveIssue("https://github.com/dotnet/runtime/issues/57333")]
    //public void CallSiteFactoryResolvesIEnumerableOfOpenGenericServiceAfterResolvingClosedImplementation()
    //{
    //    IServiceCollection descriptors = new ServiceCollection();
    //    descriptors.Add(ServiceDescriptor.Scoped(typeof(IFakeOpenGenericService<int>), typeof(FakeIntService)));
    //    descriptors.Add(ServiceDescriptor.Scoped(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>)));

    //    ServiceProvider provider = descriptors.BuildServiceProvider();

    //    IFakeOpenGenericService<int> processor = provider.GetService<IFakeOpenGenericService<int>>();
    //    IEnumerable<IFakeOpenGenericService<int>> processors = provider.GetService<IEnumerable<IFakeOpenGenericService<int>>>();

    //    Type[] implementationTypes = processors.Select(p => p.GetType()).ToArray();
    //    Assert.Equal(typeof(FakeIntService), processor.GetType());
    //    Assert.Equal(2, implementationTypes.Length);
    //    Assert.Equal(typeof(FakeIntService), implementationTypes[0]);
    //    Assert.Equal(typeof(FakeOpenGenericService<int>), implementationTypes[1]);
    //}

    private class FakeIntService : IFakeOpenGenericService<int>
    {
        public int Value => 0;
    }

    private interface IServiceG
    {
    }

    private class ServiceG
    {
        public ServiceG(IServiceG service = null) { }
    }

    private class ServiceF
    {
        public ServiceF(ServiceG service) { }
    }

    private class ServiceD
    {
        public ServiceD(IEnumerable<ServiceA> services)
        {

        }
    }

    private class ServiceA
    {
    }

    private class ServiceB
    {
        public ServiceB(ServiceA serviceA)
        {
            ServiceA = serviceA;
        }

        public ServiceA ServiceA { get; set; }
    }

    private class ServiceC
    {
        public ServiceC(ServiceB serviceB)
        {
            ServiceB = serviceB;
        }

        public ServiceB ServiceB { get; set; }
    }

    private class ServiceE
    {
        public ServiceE(ServiceD serviceD, ServiceA serviceA)
        {
            ServiceD = serviceD;
            ServiceA = serviceA;
        }

        public ServiceD ServiceD { get; set; }

        public ServiceA ServiceA { get; set; }
    }

    private class DisposableServiceA : ServiceA, IDisposable
    {
        public void Dispose()
        {
        }
    }

    private class DisposableServiceB : ServiceB, IDisposable
    {
        public DisposableServiceB(ServiceA serviceA)
            : base(serviceA)
        {
        }

        public void Dispose()
        {
        }
    }

    private class DisposableServiceC : ServiceC, IDisposable
    {
        public DisposableServiceC(ServiceB serviceB)
            : base(serviceB)
        {
        }

        public void Dispose()
        {
        }
    }

    private static object Invoke(CallSiteService callSite, ServiceProviderEngineScope scope)
    {
        return CallSiteRuntimeResolverVisitor.Instance.Resolve(callSite, scope);
    }

    private static Func<ServiceProviderEngineScope, object> CompileCallSite(CallSiteService callSite, ServiceProvider provider)
    {
        return new CallSiteExpressionResolverBuilderVisitor(provider).Build(callSite);
    }
}
