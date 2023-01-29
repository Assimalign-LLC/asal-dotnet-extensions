using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assimalign.Extensions.DependencyInjection.Specification.Fakes;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection.Specification
{
    public abstract partial class DependencyInjectionSpecificationTests
    {
        protected abstract IServiceProvider CreateServiceProvider(IServiceProviderBuilder providerBuilder);

        protected virtual IServiceProvider CreateServiceProvider(IServiceProviderBuilder builder, ServiceProviderOptions options)
        {
            return null;
        }

        [Fact]
        public void ServicesRegisteredWithImplementationTypeCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service = provider.GetService<IFakeService>();

            // Assert
            Assert.NotNull(service);
            Assert.IsType<FakeService>(service);
        }

        [Fact]
        public void ServicesRegisteredWithImplementationType_ReturnDifferentInstancesPerResolution_ForTransientServices()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(providerBuilder); ;


            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.IsType<FakeService>(service1);
            Assert.IsType<FakeService>(service2);
            Assert.NotSame(service1, service2);
        }

        [Fact]
        public void ServicesRegisteredWithImplementationType_ReturnSameInstancesPerResolution_ForSingletons()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddSingleton(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.IsType<FakeService>(service1);
            Assert.IsType<FakeService>(service2);
            Assert.Same(service1, service2);
        }

        [Fact]
        public void ServiceInstanceCanBeResolved()
        {
            // Arrange
            var providerbuilder = new ServiceProviderBuilder();
            var instance = new FakeService();
            providerbuilder.AddSingleton(typeof(IFakeServiceInstance), instance);
            var provider = CreateServiceProvider(providerbuilder);

            // Act
            var service = provider.GetService<IFakeServiceInstance>();

            // Assert
            Assert.Same(instance, service);
        }

        [Fact]
        public void TransientServiceCanBeResolvedFromProvider()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service1 = provider.GetService<IFakeService>();
            var service2 = provider.GetService<IFakeService>();

            // Assert
            Assert.NotNull(service1);
            Assert.NotSame(service1, service2);
        }

        [Fact]
        public void TransientServiceCanBeResolvedFromScope()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service1 = provider.GetService<IFakeService>();

            using (var scope = provider.CreateScope())
            {
                var scopedService1 = scope.ServiceProvider.GetService<IFakeService>();
                var scopedService2 = scope.ServiceProvider.GetService<IFakeService>();

                // Assert
                Assert.NotSame(service1, scopedService1);
                Assert.NotSame(service1, scopedService2);
                Assert.NotSame(scopedService1, scopedService2);
            }
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        public void NonSingletonService_WithInjectedProvider_ResolvesScopeProvider(ServiceLifetime lifetime)
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder().AddScoped<IFakeService, FakeService>();
            providerBuilder.Add(new ServiceDescriptor(typeof(ClassWithServiceProvider), typeof(ClassWithServiceProvider), lifetime));
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            IFakeService fakeServiceFromScope1 = null;
            IFakeService otherFakeServiceFromScope1 = null;
            IFakeService fakeServiceFromScope2 = null;
            IFakeService otherFakeServiceFromScope2 = null;

            using (var scope1 = provider.CreateScope())
            {
                var serviceWithProvider = scope1.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
                fakeServiceFromScope1 = serviceWithProvider.ServiceProvider.GetRequiredService<IFakeService>();

                serviceWithProvider = scope1.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
                otherFakeServiceFromScope1 = serviceWithProvider.ServiceProvider.GetRequiredService<IFakeService>();
            }

            using (var scope2 = provider.CreateScope())
            {
                var serviceWithProvider = scope2.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
                fakeServiceFromScope2 = serviceWithProvider.ServiceProvider.GetRequiredService<IFakeService>();

                serviceWithProvider = scope2.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
                otherFakeServiceFromScope2 = serviceWithProvider.ServiceProvider.GetRequiredService<IFakeService>();
            }

            // Assert
            Assert.Same(fakeServiceFromScope1, otherFakeServiceFromScope1);
            Assert.Same(fakeServiceFromScope2, otherFakeServiceFromScope2);
            Assert.NotSame(fakeServiceFromScope1, fakeServiceFromScope2);
        }

        [Fact]
        public void SingletonServiceCanBeResolvedFromScope()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddSingleton<ClassWithServiceProvider>();

            var provider = CreateServiceProvider(providerBuilder);

            // Act
            IServiceProvider scopedSp1 = null;
            IServiceProvider scopedSp2 = null;
            ClassWithServiceProvider instance1 = null;
            ClassWithServiceProvider instance2 = null;

            using (var scope1 = provider.CreateScope())
            {
                scopedSp1 = scope1.ServiceProvider;
                instance1 = scope1.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
            }

            using (var scope2 = provider.CreateScope())
            {
                scopedSp2 = scope2.ServiceProvider;
                instance2 = scope2.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
            }

            // Assert
            Assert.Same(instance1.ServiceProvider, instance2.ServiceProvider);
            Assert.NotSame(instance1.ServiceProvider, scopedSp1);
            Assert.NotSame(instance2.ServiceProvider, scopedSp2);
        }

        [Fact]
        public void SingleServiceCanBeIEnumerableResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(providerBuilder);


            // Act
            var services = provider.GetService<IEnumerable<IFakeService>>();

            // Assert
            Assert.NotNull(services);
            var service = Assert.Single(services);
            Assert.IsType<FakeService>(service);
        }

        [Fact]
        public void MultipleServiceCanBeIEnumerableResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeMultipleService), typeof(FakeOneMultipleService))
                .AddTransient(typeof(IFakeMultipleService), typeof(FakeTwoMultipleService));

            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var services = provider.GetService<IEnumerable<IFakeMultipleService>>();

            // Assert
            Assert.Collection(services.OrderBy(s => s.GetType().FullName),
                service => Assert.IsType<FakeOneMultipleService>(service),
                service => Assert.IsType<FakeTwoMultipleService>(service));
        }

        [Fact]
        public void RegistrationOrderIsPreservedWhenServicesAreIEnumerableResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeMultipleService), typeof(FakeOneMultipleService))
                .AddTransient(typeof(IFakeMultipleService), typeof(FakeTwoMultipleService));

            var provider = CreateServiceProvider(providerBuilder);

            providerBuilder.Services.Reverse();

            var providerReversed = CreateServiceProvider(providerBuilder);

            // Act
            var services = provider.GetService<IEnumerable<IFakeMultipleService>>();
            var servicesReversed = providerReversed.GetService<IEnumerable<IFakeMultipleService>>();

            // Assert
            Assert.Collection(services,
                service => Assert.IsType<FakeOneMultipleService>(service),
                service => Assert.IsType<FakeTwoMultipleService>(service));

            Assert.Collection(servicesReversed,
                service => Assert.IsType<FakeTwoMultipleService>(service),
                service => Assert.IsType<FakeOneMultipleService>(service));
        }

        [Fact]
        public void OuterServiceCanHaveOtherServicesInjected()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            var fakeService = new FakeService();
            providerBuilder.AddTransient<IFakeOuterService, FakeOuterService>();
            providerBuilder.AddSingleton<IFakeService>(fakeService);
            providerBuilder.AddTransient<IFakeMultipleService, FakeOneMultipleService>();
            providerBuilder.AddTransient<IFakeMultipleService, FakeTwoMultipleService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var services = provider.GetService<IFakeOuterService>();

            // Assert
            Assert.Same(fakeService, services.SingleService);
            Assert.Collection(services.MultipleServices.OrderBy(s => s.GetType().FullName),
                service => Assert.IsType<FakeOneMultipleService>(service),
                service => Assert.IsType<FakeTwoMultipleService>(service));
        }

        [Fact]
        public void FactoryServicesCanBeCreatedByGetService()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient<IFakeService, FakeService>();
            providerBuilder.AddTransient<IFactoryService>(p =>
            {
                var fakeService = p.GetRequiredService<IFakeService>();
                return new TransientFactoryService
                {
                    FakeService = fakeService,
                    Value = 42
                };
            });
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service = provider.GetService<IFactoryService>();

            // Assert
            Assert.NotNull(service);
            Assert.Equal(42, service.Value);
            Assert.NotNull(service.FakeService);
            Assert.IsType<FakeService>(service.FakeService);
        }

        [Fact]
        public void FactoryServicesAreCreatedAsPartOfCreatingObjectGraph()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient<IFakeService, FakeService>();
            providerBuilder.AddTransient<IFactoryService>(p =>
            {
                var fakeService = p.GetService<IFakeService>();
                return new TransientFactoryService
                {
                    FakeService = fakeService,
                    Value = 42
                };
            });
            providerBuilder.AddScoped(p =>
            {
                var fakeService = p.GetService<IFakeService>();
                return new ScopedFactoryService
                {
                    FakeService = fakeService,
                };
            });
            providerBuilder.AddTransient<ServiceAcceptingFactoryService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service1 = provider.GetService<ServiceAcceptingFactoryService>();
            var service2 = provider.GetService<ServiceAcceptingFactoryService>();

            // Assert
            Assert.Equal(42, service1.TransientService.Value);
            Assert.NotNull(service1.TransientService.FakeService);

            Assert.Equal(42, service2.TransientService.Value);
            Assert.NotNull(service2.TransientService.FakeService);

            Assert.NotNull(service1.ScopedService.FakeService);

            // Verify scoping works
            Assert.NotSame(service1.TransientService, service2.TransientService);
            Assert.Same(service1.ScopedService, service2.ScopedService);
        }

        [Fact]
        public void LastServiceReplacesPreviousServices()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient<IFakeMultipleService, FakeOneMultipleService>();
            providerBuilder.AddTransient<IFakeMultipleService, FakeTwoMultipleService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service = provider.GetService<IFakeMultipleService>();

            // Assert
            Assert.IsType<FakeTwoMultipleService>(service);
        }

        [Fact]
        public void SingletonServiceCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service1 = provider.GetService<IFakeSingletonService>();
            var service2 = provider.GetService<IFakeSingletonService>();

            // Assert
            Assert.NotNull(service1);
            Assert.Same(service1, service2);
        }

        [Fact]
        public void ServiceProviderRegistersServiceScopeFactory()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var scopeFactory = provider.GetService<IServiceScopeFactory>();

            // Assert
            Assert.NotNull(scopeFactory);
        }

        [Fact]
        public void ServiceScopeFactoryIsSingleton()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var scopeFactory1 = provider.GetService<IServiceScopeFactory>();
            var scopeFactory2 = provider.GetService<IServiceScopeFactory>();
            using (var scope = provider.CreateScope())
            {
                var scopeFactory3 = scope.ServiceProvider.GetService<IServiceScopeFactory>();

                // Assert
                Assert.Same(scopeFactory1, scopeFactory2);
                Assert.Same(scopeFactory1, scopeFactory3);
            }
        }

        [Fact]
        public void ScopedServiceCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            using (var scope = provider.CreateScope())
            {
                var providerScopedService = provider.GetService<IFakeScopedService>();
                var scopedService1 = scope.ServiceProvider.GetService<IFakeScopedService>();
                var scopedService2 = scope.ServiceProvider.GetService<IFakeScopedService>();

                // Assert
                Assert.NotSame(providerScopedService, scopedService1);
                Assert.Same(scopedService1, scopedService2);
            }
        }

        [Fact]
        public void NestedScopedServiceCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            using (var outerScope = provider.CreateScope())
            using (var innerScope = outerScope.ServiceProvider.CreateScope())
            {
                var outerScopedService = outerScope.ServiceProvider.GetService<IFakeScopedService>();
                var innerScopedService = innerScope.ServiceProvider.GetService<IFakeScopedService>();

                // Assert
                Assert.NotNull(outerScopedService);
                Assert.NotNull(innerScopedService);
                Assert.NotSame(outerScopedService, innerScopedService);
            }
        }

        [Fact]
        public void ScopedServices_FromCachedScopeFactory_CanBeResolvedAndDisposed()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);
            var cachedScopeFactory = provider.GetService<IServiceScopeFactory>();

            // Act
            for (var i = 0; i < 3; i++)
            {
                FakeService outerScopedService;
                using (var outerScope = cachedScopeFactory.CreateScope())
                {
                    FakeService innerScopedService;
                    using (var innerScope = outerScope.ServiceProvider.CreateScope())
                    {
                        outerScopedService = outerScope.ServiceProvider.GetService<IFakeScopedService>() as FakeService;
                        innerScopedService = innerScope.ServiceProvider.GetService<IFakeScopedService>() as FakeService;

                        // Assert
                        Assert.NotNull(outerScopedService);
                        Assert.NotNull(innerScopedService);
                        Assert.NotSame(outerScopedService, innerScopedService);
                    }

                    Assert.False(outerScopedService.Disposed);
                    Assert.True(innerScopedService.Disposed);
                }

                Assert.True(outerScopedService.Disposed);
            }
        }

        [Fact]
        public void ScopesAreFlatNotHierarchical()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var outerScope = provider.CreateScope();
            using var innerScope = outerScope.ServiceProvider.CreateScope();
            outerScope.Dispose();
            var innerScopedService = innerScope.ServiceProvider.GetService<IFakeSingletonService>();

            // Assert
            Assert.NotNull(innerScopedService);
        }

        [Fact]
        public void ServiceProviderIsDisposable()
        {
            var provider = CreateServiceProvider(new ServiceProviderBuilder());

            Assert.IsAssignableFrom<IDisposable>(provider);
        }

        [Fact]
        public void DisposingScopeDisposesService()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            providerBuilder.AddScoped<IFakeScopedService, FakeService>();
            providerBuilder.AddTransient<IFakeService, FakeService>();

            var provider = CreateServiceProvider(providerBuilder);
            FakeService disposableService;
            FakeService transient1;
            FakeService transient2;
            FakeService singleton;

            // Act and Assert
            var transient3 = Assert.IsType<FakeService>(provider.GetService<IFakeService>());
            using (var scope = provider.CreateScope())
            {
                disposableService = (FakeService)scope.ServiceProvider.GetService<IFakeScopedService>();
                transient1 = (FakeService)scope.ServiceProvider.GetService<IFakeService>();
                transient2 = (FakeService)scope.ServiceProvider.GetService<IFakeService>();
                singleton = (FakeService)scope.ServiceProvider.GetService<IFakeSingletonService>();

                Assert.False(disposableService.Disposed);
                Assert.False(transient1.Disposed);
                Assert.False(transient2.Disposed);
                Assert.False(singleton.Disposed);
            }

            Assert.True(disposableService.Disposed);
            Assert.True(transient1.Disposed);
            Assert.True(transient2.Disposed);
            Assert.False(singleton.Disposed);

            (provider as IDisposable).Dispose();

            Assert.True(singleton.Disposed);
            Assert.True(transient3.Disposed);
        }

        [Fact]
        public void SelfResolveThenDispose()
        {
            // Arrange
            var collection = new ServiceProviderBuilder();
            var provider = CreateServiceProvider(collection);

            // Act
            var serviceProvider = provider.GetService<IServiceProvider>();

            // Assert
            Assert.NotNull(serviceProvider);
            (provider as IDisposable).Dispose();
        }

        [Fact]
        public void SafelyDisposeNestedProviderReferences()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient<ClassWithNestedReferencesToProvider>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var nester = provider.GetService<ClassWithNestedReferencesToProvider>();

            // Assert
            Assert.NotNull(nester);
            nester.Dispose();
        }

        [Fact]
        public void SingletonServicesComeFromRootProvider()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);
            FakeService disposableService1;
            FakeService disposableService2;

            // Act and Assert
            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IFakeSingletonService>();
                disposableService1 = Assert.IsType<FakeService>(service);
                Assert.False(disposableService1.Disposed);
            }

            Assert.False(disposableService1.Disposed);

            using (var scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IFakeSingletonService>();
                disposableService2 = Assert.IsType<FakeService>(service);
                Assert.False(disposableService2.Disposed);
            }

            Assert.False(disposableService2.Disposed);
            Assert.Same(disposableService1, disposableService2);
        }

        [Fact]
        public void NestedScopedServiceCanBeResolvedWithNoFallbackProvider()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            using (var outerScope = provider.CreateScope())
            using (var innerScope = outerScope.ServiceProvider.CreateScope())
            {
                var outerScopedService = outerScope.ServiceProvider.GetService<IFakeScopedService>();
                var innerScopedService = innerScope.ServiceProvider.GetService<IFakeScopedService>();

                // Assert
                Assert.NotSame(outerScopedService, innerScopedService);
            }
        }

        [Fact]
        public void OpenGenericServicesCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var genericService = provider.GetService<IFakeOpenGenericService<IFakeSingletonService>>();
            var singletonService = provider.GetService<IFakeSingletonService>();

            // Assert
            Assert.Same(singletonService, genericService.Value);
        }

        [Fact]
        public void ConstrainedOpenGenericServicesCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ConstrainedFakeOpenGenericService<>));
            var poco = new PocoClass();
            providerBuilder.AddSingleton(poco);
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);
            // Act
            var allServices = provider.GetServices<IFakeOpenGenericService<PocoClass>>().ToList();
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<IFakeSingletonService>>().ToList();
            var singletonService = provider.GetService<IFakeSingletonService>();
            // Assert
            Assert.Equal(2, allServices.Count);
            Assert.Same(poco, allServices[0].Value);
            Assert.Same(poco, allServices[1].Value);
            Assert.Equal(1, constrainedServices.Count);
            Assert.Same(singletonService, constrainedServices[0].Value);
        }

        [Fact]
        public void ConstrainedOpenGenericServicesReturnsEmptyWithNoMatches()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ConstrainedFakeOpenGenericService<>));
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);
            // Act
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<IFakeSingletonService>>().ToList();
            // Assert
            Assert.Equal(0, constrainedServices.Count);
        }

        [Fact]
        public void InterfaceConstrainedOpenGenericServicesCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ClassWithInterfaceConstraint<>));
            var enumerableVal = new ClassImplementingIEnumerable();
            providerBuilder.AddSingleton(enumerableVal);
            providerBuilder.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(providerBuilder);
            // Act
            var allServices = provider.GetServices<IFakeOpenGenericService<ClassImplementingIEnumerable>>().ToList();
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<IFakeSingletonService>>().ToList();
            var singletonService = provider.GetService<IFakeSingletonService>();
            // Assert
            Assert.Equal(2, allServices.Count);
            Assert.Same(enumerableVal, allServices[0].Value);
            Assert.Same(enumerableVal, allServices[1].Value);
            Assert.Equal(1, constrainedServices.Count);
            Assert.Same(singletonService, constrainedServices[0].Value);
        }

        [Fact]
        public void AbstractClassConstrainedOpenGenericServicesCanBeResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ClassWithAbstractClassConstraint<>));
            var poco = new PocoClass();
            providerBuilder.AddSingleton(poco);
            var classInheritingClassInheritingAbstractClass = new ClassInheritingClassInheritingAbstractClass();
            providerBuilder.AddSingleton(classInheritingClassInheritingAbstractClass);
            var provider = CreateServiceProvider(providerBuilder);
            // Act
            var allServices = provider.GetServices<IFakeOpenGenericService<ClassInheritingClassInheritingAbstractClass>>().ToList();
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<PocoClass>>().ToList();
            // Assert
            Assert.Equal(2, allServices.Count);
            Assert.Same(classInheritingClassInheritingAbstractClass, allServices[0].Value);
            Assert.Same(classInheritingClassInheritingAbstractClass, allServices[1].Value);
            Assert.Equal(1, constrainedServices.Count);
            Assert.Same(poco, constrainedServices[0].Value);
        }

        [Fact]
        public void ClosedServicesPreferredOverOpenGenericServices()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<PocoClass>), typeof(FakeService));
            providerBuilder.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            providerBuilder.AddSingleton<PocoClass>();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service = provider.GetService<IFakeOpenGenericService<PocoClass>>();

            // Assert
            Assert.IsType<FakeService>(service);
        }

        [Fact]
        public void AttemptingToResolveNonexistentServiceReturnsNull()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var service = provider.GetService<INonexistentService>();

            // Assert
            Assert.Null(service);
        }

        [Fact]
        public void NonexistentServiceCanBeIEnumerableResolved()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            var provider = CreateServiceProvider(providerBuilder);

            // Act
            var services = provider.GetService<IEnumerable<INonexistentService>>();

            // Assert
            Assert.Empty(services);
        }

        //public static TheoryData ServiceContainerPicksConstructorWithLongestMatchesData
        //{
        //    get
        //    {
        //        var fakeService = new FakeService();
        //        var multipleService = new FakeService();
        //        var factoryService = new TransientFactoryService();
        //        var scopedService = new FakeService();

        //        return new TheoryData<IServiceCollection, TypeWithSupersetConstructors>
        //        {
        //            {
        //                new ServiceProviderBuilder()
        //                    .AddSingleton<IFakeService>(fakeService),
        //                new TypeWithSupersetConstructors(fakeService)
        //            },
        //            {
        //                new ServiceProviderBuilder()
        //                    .AddSingleton<IFactoryService>(factoryService),
        //                new TypeWithSupersetConstructors(factoryService)
        //            },
        //            {
        //                new ServiceProviderBuilder()
        //                    .AddSingleton<IFakeService>(fakeService)
        //                    .AddSingleton<IFactoryService>(factoryService),
        //               new TypeWithSupersetConstructors(fakeService, factoryService)
        //            },
        //            {
        //                new ServiceProviderBuilder()
        //                    .AddSingleton<IFakeService>(fakeService)
        //                    .AddSingleton<IFakeMultipleService>(multipleService)
        //                    .AddSingleton<IFactoryService>(factoryService),
        //               new TypeWithSupersetConstructors(fakeService, multipleService, factoryService)
        //            },
        //            {
        //                new ServiceProviderBuilder()
        //                    .AddSingleton<IFakeService>(fakeService)
        //                    .AddSingleton<IFakeMultipleService>(multipleService)
        //                    .AddSingleton<IFakeScopedService>(scopedService)
        //                    .AddSingleton<IFactoryService>(factoryService),
        //               new TypeWithSupersetConstructors(multipleService, factoryService, fakeService, scopedService)
        //            }
        //        };
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(ServiceContainerPicksConstructorWithLongestMatchesData))]
        //public void ServiceContainerPicksConstructorWithLongestMatches(
        //    IServiceCollection serviceCollection,
        //    TypeWithSupersetConstructors expected)
        //{
        //    // Arrange
        //    serviceproviderBuilder.AddTransient<TypeWithSupersetConstructors>();
        //    var serviceProvider = CreateServiceProvider(serviceCollection);

        //    // Act
        //    var actual = serviceProvider.GetService<TypeWithSupersetConstructors>();

        //    // Assert
        //    Assert.NotNull(actual);
        //    Assert.Same(expected.Service, actual.Service);
        //    Assert.Same(expected.FactoryService, actual.FactoryService);
        //    Assert.Same(expected.MultipleService, actual.MultipleService);
        //    Assert.Same(expected.ScopedService, actual.ScopedService);
        //}

        [Fact]
        public void DisposesInReverseOrderOfCreation()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            providerBuilder.AddSingleton<FakeDisposeCallback>();
            providerBuilder.AddTransient<IFakeOuterService, FakeDisposableCallbackOuterService>();
            providerBuilder.AddSingleton<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            providerBuilder.AddScoped<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            providerBuilder.AddTransient<IFakeMultipleService, FakeDisposableCallbackInnerService>();
            providerBuilder.AddSingleton<IFakeService, FakeDisposableCallbackInnerService>();
            var serviceProvider = CreateServiceProvider(providerBuilder);

            var callback = serviceProvider.GetService<FakeDisposeCallback>();
            var outer = serviceProvider.GetService<IFakeOuterService>();
            var multipleServices = outer.MultipleServices.ToArray();

            // Act
            (serviceProvider as IDisposable).Dispose();

            // Assert
            Assert.Equal(outer, callback.Disposed[0]);
            Assert.Equal(multipleServices.Reverse(), callback.Disposed.Skip(1).Take(3).OfType<IFakeMultipleService>());
            Assert.Equal(outer.SingleService, callback.Disposed[4]);
        }

        [Fact]
        public void ResolvesMixedOpenClosedGenericsAsEnumerable()
        {
            // Arrange
            var providerBuilder = new ServiceProviderBuilder();
            var instance = new FakeOpenGenericService<PocoClass>(null);

            providerBuilder.AddTransient<PocoClass, PocoClass>();
            providerBuilder.AddSingleton(typeof(IFakeOpenGenericService<PocoClass>), typeof(FakeService));
            providerBuilder.AddSingleton(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            providerBuilder.AddSingleton<IFakeOpenGenericService<PocoClass>>(instance);

            var serviceProvider = CreateServiceProvider(providerBuilder);

            var enumerable = serviceProvider.GetService<IEnumerable<IFakeOpenGenericService<PocoClass>>>().ToArray();

            // Assert
            Assert.Equal(3, enumerable.Length);
            Assert.NotNull(enumerable[0]);
            Assert.NotNull(enumerable[1]);
            Assert.NotNull(enumerable[2]);

            Assert.Equal(instance, enumerable[2]);
            Assert.IsType<FakeService>(enumerable[0]);
        }

        [Theory]
        [InlineData(typeof(IFakeService), typeof(FakeService), typeof(IFakeService), ServiceLifetime.Scoped)]
        [InlineData(typeof(IFakeService), typeof(FakeService), typeof(IFakeService), ServiceLifetime.Singleton)]
        [InlineData(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>), typeof(IFakeOpenGenericService<IServiceProvider>), ServiceLifetime.Scoped)]
        [InlineData(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>), typeof(IFakeOpenGenericService<IServiceProvider>), ServiceLifetime.Singleton)]
        public void ResolvesDifferentInstancesForServiceWhenResolvingEnumerable(Type serviceType, Type implementation, Type resolve, ServiceLifetime lifetime)
        {
            // Arrange
            var builder = new ServiceProviderBuilder();
            var serviceDescriptor = new ServiceDescriptor[]
            {
                ServiceDescriptor.Describe(serviceType, implementation, lifetime),
                ServiceDescriptor.Describe(serviceType, implementation, lifetime),
                ServiceDescriptor.Describe(serviceType, implementation, lifetime)
            };
            
            foreach (var desc in serviceDescriptor)
            {
                builder.Add(desc);
            }

            var serviceProvider = CreateServiceProvider(builder);
            using (var scope = serviceProvider.CreateScope())
            {
                var enumerable = (scope.ServiceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(resolve)) as IEnumerable)
                    .OfType<object>().ToArray();
                var service = scope.ServiceProvider.GetService(resolve);

                // Assert
                Assert.Equal(3, enumerable.Length);
                Assert.NotNull(enumerable[0]);
                Assert.NotNull(enumerable[1]);
                Assert.NotNull(enumerable[2]);

                Assert.NotEqual(enumerable[0], enumerable[1]);
                Assert.NotEqual(enumerable[1], enumerable[2]);
                Assert.Equal(service, enumerable[2]);
            }
        }
    }
}
