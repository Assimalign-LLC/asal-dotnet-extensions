using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class ServiceProviderFactoryTests
    {

        enum ServiceProviders
        {
            Provider1,
            Provider2
        }


        [Fact]
        public void Test()
        {
            var factory = ServiceProviderFactory.Create(builder =>
            {
                builder
                    .AddServiceProvider(ServiceProviders.Provider1, services =>
                    {
                        
                    })
                    .AddServiceProvider(ServiceProviders.Provider2, services =>
                    {

                    });
            });

            var provider1 = factory.Create(ServiceProviders.Provider1);
            var provider2 = factory.Create(ServiceProviders.Provider2);

            var factory1 = provider1.GetRequiredService<IServiceProviderFactory>();
            var factory2 = provider2.GetRequiredService<IServiceProviderFactory>();

            Assert.Same(factory1, factory2);
        }
    }
}
