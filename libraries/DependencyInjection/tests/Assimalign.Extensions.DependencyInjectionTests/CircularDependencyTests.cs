


using System;
using System.Collections.Generic;
using Assimalign.Extensions.DependencyInjection.Tests.Fakes;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class CircularDependencyTests
    {
        [Fact]
        public void SelfCircularDependency()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependency'." +
                                  Environment.NewLine +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependency -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependency";

            var serviceProvider = new ServiceCollection()
                .AddTransient<SelfCircularDependency>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<SelfCircularDependency>());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void SelfCircularDependencyInEnumerable()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependency'." +
                                  Environment.NewLine +
                                  "System.Collections.Generic.IEnumerable<Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependency> -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependency -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependency";

            var serviceProvider = new ServiceCollection()
                .AddTransient<SelfCircularDependency>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<IEnumerable<SelfCircularDependency>>());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void SelfCircularDependencyGenericDirect()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyGeneric<string>'." +
                                  Environment.NewLine +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyGeneric<string> -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyGeneric<string>";

            var serviceProvider = new ServiceCollection()
                .AddTransient<SelfCircularDependencyGeneric<string>>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<SelfCircularDependencyGeneric<string>>());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void SelfCircularDependencyGenericIndirect()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyGeneric<string>'." +
                                  Environment.NewLine +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyGeneric<int> -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyGeneric<string> -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyGeneric<string>";

            var serviceProvider = new ServiceCollection()
                .AddTransient<SelfCircularDependencyGeneric<int>>()
                .AddTransient<SelfCircularDependencyGeneric<string>>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<SelfCircularDependencyGeneric<int>>());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void NoCircularDependencyGeneric()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(new SelfCircularDependencyGeneric<string>())
                .AddTransient<SelfCircularDependencyGeneric<int>>()
                .BuildServiceProvider();

            // This will not throw because we are creating an instance of the first time
            // using the parameterless constructor which has no circular dependency
            var resolvedService = serviceProvider.GetRequiredService<SelfCircularDependencyGeneric<int>>();
            Assert.NotNull(resolvedService);
        }

        [Fact]
        public void SelfCircularDependencyWithInterface()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.ISelfCircularDependencyWithInterface'." +
                                  Environment.NewLine +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyWithInterface -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.ISelfCircularDependencyWithInterface" +
                                  "(Assimalign.Extensions.DependencyInjection.Tests.Fakes.SelfCircularDependencyWithInterface) -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.ISelfCircularDependencyWithInterface";

            var serviceProvider = new ServiceCollection()
                .AddTransient<ISelfCircularDependencyWithInterface, SelfCircularDependencyWithInterface>()
                .AddTransient<SelfCircularDependencyWithInterface>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<SelfCircularDependencyWithInterface>());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void DirectCircularDependency()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyA'." +
                                  Environment.NewLine +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyA -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyB -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyA";

            var serviceProvider = new ServiceCollection()
                .AddSingleton<DirectCircularDependencyA>()
                .AddSingleton<DirectCircularDependencyB>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<DirectCircularDependencyA>());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void IndirectCircularDependency()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.IndirectCircularDependencyA'." +
                                  Environment.NewLine +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.IndirectCircularDependencyA -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.IndirectCircularDependencyB -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.IndirectCircularDependencyC -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.IndirectCircularDependencyA";

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IndirectCircularDependencyA>()
                .AddTransient<IndirectCircularDependencyB>()
                .AddTransient<IndirectCircularDependencyC>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<IndirectCircularDependencyA>());

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void NoCircularDependencySameTypeMultipleTimes()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<NoCircularDependencySameTypeMultipleTimesA>()
                .AddTransient<NoCircularDependencySameTypeMultipleTimesB>()
                .AddTransient<NoCircularDependencySameTypeMultipleTimesC>()
                .BuildServiceProvider();

            var resolvedService = serviceProvider.GetRequiredService<NoCircularDependencySameTypeMultipleTimesA>();
            Assert.NotNull(resolvedService);
        }

        [Fact]
        public void DependencyOnCircularDependency()
        {
            var expectedMessage = "A circular dependency was detected for the service of type " +
                                  "'Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyA'." +
                                  Environment.NewLine +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.DependencyOnCircularDependency -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyA -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyB -> " +
                                  "Assimalign.Extensions.DependencyInjection.Tests.Fakes.DirectCircularDependencyA";

            var serviceProvider = new ServiceCollection()
                .AddTransient<DependencyOnCircularDependency>()
                .AddTransient<DirectCircularDependencyA>()
                .AddTransient<DirectCircularDependencyB>()
                .BuildServiceProvider();

            var exception = Assert.Throws<InvalidOperationException>(() =>
                serviceProvider.GetRequiredService<DependencyOnCircularDependency>());

            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
