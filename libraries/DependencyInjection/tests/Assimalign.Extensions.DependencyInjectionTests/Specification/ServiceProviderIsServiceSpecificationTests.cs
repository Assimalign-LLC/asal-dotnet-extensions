using System;
using System.Collections.Generic;
using System.Text;
using Assimalign.Extensions.DependencyInjection.MockObjects;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection.Specification
{


    public abstract partial class DependencyInjectionSpecificationTests
    {
        public virtual bool SupportsIServiceProviderIsService => true;

        [Fact]
        public void ExplicitServiceRegisterationWithIsService()
        {
            if (!SupportsIServiceProviderIsService)
            {
                return;
            }

            // Arrange
            var provider = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService))
                .Build();   

            // Act
            var serviceProviderIsService = provider.GetService<IServiceLookup>();

            // Assert
            Assert.NotNull(serviceProviderIsService);
            Assert.True(serviceProviderIsService.IsService(typeof(IFakeService)));
            Assert.False(serviceProviderIsService.IsService(typeof(FakeService)));
        }

        [Fact]
        public void OpenGenericsWithIsService()
        {
            if (!SupportsIServiceProviderIsService)
            {
                return;
            }

            // Arrange
            var provider = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>))
                .Build();
 
            // Act
            var serviceProviderIsService = provider.GetService<IServiceLookup>();

            // Assert
            Assert.NotNull(serviceProviderIsService);
            Assert.True(serviceProviderIsService.IsService(typeof(IFakeOpenGenericService<IFakeService>)));
            Assert.False(serviceProviderIsService.IsService(typeof(IFakeOpenGenericService<>)));
        }

        [Fact]
        public void ClosedGenericsWithIsService()
        {
            if (!SupportsIServiceProviderIsService)
            {
                return;
            }

            // Arrange
            var provider = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeOpenGenericService<IFakeService>), typeof(FakeOpenGenericService<IFakeService>))
                .Build();

            // Act
            var serviceProviderIsService = provider.GetService<IServiceLookup>();

            // Assert
            Assert.NotNull(serviceProviderIsService);
            Assert.True(serviceProviderIsService.IsService(typeof(IFakeOpenGenericService<IFakeService>)));
        }

        [Fact]
        public void IEnumerableWithIsServiceAlwaysReturnsTrue()
        {
            if (!SupportsIServiceProviderIsService)
            {
                return;
            }

            // Arrange
            var provider = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService))
                .Build();

            // Act
            var serviceProviderIsService = provider.GetService<IServiceLookup>();

            // Assert
            Assert.NotNull(serviceProviderIsService);
            Assert.True(serviceProviderIsService.IsService(typeof(IEnumerable<IFakeService>)));
            Assert.True(serviceProviderIsService.IsService(typeof(IEnumerable<FakeService>)));
            Assert.False(serviceProviderIsService.IsService(typeof(IEnumerable<>)));
        }

        [Fact]
        public void BuiltInServicesWithIsServiceReturnsTrue()
        {
            if (!SupportsIServiceProviderIsService)
            {
                return;
            }

            // Arrange
            var provider = new ServiceProviderBuilder()
                .AddTransient(typeof(IFakeService), typeof(FakeService))
                .Build();

            // Act
            var serviceProviderIsService = provider.GetService<IServiceLookup>();

            // Assert
            Assert.NotNull(serviceProviderIsService);
            Assert.True(serviceProviderIsService.IsService(typeof(IServiceProvider)));
            Assert.True(serviceProviderIsService.IsService(typeof(IServiceScopeFactory)));
            Assert.True(serviceProviderIsService.IsService(typeof(IServiceLookup)));
        }
    }
}
