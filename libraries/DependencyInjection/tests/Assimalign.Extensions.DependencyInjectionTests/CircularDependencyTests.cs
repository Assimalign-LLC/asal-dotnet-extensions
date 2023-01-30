


using System;
using System.Collections.Generic;
using Assimalign.Extensions.DependencyInjection.MockObjects;
using Xunit;

namespace Assimalign.Extensions.DependencyInjection;

public class CircularDependencyTests
{
    [Fact]
    public void SelfCircularDependency()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependency'." +
                              Environment.NewLine +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependency -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependency";

        var serviceProvider = new ServiceProviderBuilder()
            .AddTransient<SelfCircularDependency>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<SelfCircularDependency>());

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void SelfCircularDependencyInEnumerable()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependency'." +
                              Environment.NewLine +
                              "System.Collections.Generic.IEnumerable<Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependency> -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependency -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependency";

        var serviceProvider = new ServiceProviderBuilder()
            .AddTransient<SelfCircularDependency>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<IEnumerable<SelfCircularDependency>>());

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void SelfCircularDependencyGenericDirect()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyGeneric<string>'." +
                              Environment.NewLine +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyGeneric<string> -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyGeneric<string>";

        var serviceProvider = new ServiceProviderBuilder()
            .AddTransient<SelfCircularDependencyGeneric<string>>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<SelfCircularDependencyGeneric<string>>());

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void SelfCircularDependencyGenericIndirect()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyGeneric<string>'." +
                              Environment.NewLine +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyGeneric<int> -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyGeneric<string> -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyGeneric<string>";

        var serviceProvider = new ServiceProviderBuilder()
            .AddTransient<SelfCircularDependencyGeneric<int>>()
            .AddTransient<SelfCircularDependencyGeneric<string>>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<SelfCircularDependencyGeneric<int>>());

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void NoCircularDependencyGeneric()
    {
        var serviceProvider = new ServiceProviderBuilder()
            .AddSingleton(new SelfCircularDependencyGeneric<string>())
            .AddTransient<SelfCircularDependencyGeneric<int>>()
            .Build();

        // This will not throw because we are creating an instance of the first time
        // using the parameterless constructor which has no circular dependency
        var resolvedService = serviceProvider.GetRequiredService<SelfCircularDependencyGeneric<int>>();
        Assert.NotNull(resolvedService);
    }

    [Fact]
    public void SelfCircularDependencyWithInterface()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.ISelfCircularDependencyWithInterface'." +
                              Environment.NewLine +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyWithInterface -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.ISelfCircularDependencyWithInterface" +
                              "(Assimalign.Extensions.DependencyInjection.MockObjects.SelfCircularDependencyWithInterface) -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.ISelfCircularDependencyWithInterface";

        var serviceProvider = new ServiceProviderBuilder()
            .AddTransient<ISelfCircularDependencyWithInterface, SelfCircularDependencyWithInterface>()
            .AddTransient<SelfCircularDependencyWithInterface>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<SelfCircularDependencyWithInterface>());

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void DirectCircularDependency()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyA'." +
                              Environment.NewLine +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyA -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyB -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyA";

        var serviceProvider = new ServiceProviderBuilder()
            .AddSingleton<DirectCircularDependencyA>()
            .AddSingleton<DirectCircularDependencyB>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<DirectCircularDependencyA>());

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void IndirectCircularDependency()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.IndirectCircularDependencyA'." +
                              Environment.NewLine +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.IndirectCircularDependencyA -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.IndirectCircularDependencyB -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.IndirectCircularDependencyC -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.IndirectCircularDependencyA";

        var serviceProvider = new ServiceProviderBuilder()
            .AddSingleton<IndirectCircularDependencyA>()
            .AddTransient<IndirectCircularDependencyB>()
            .AddTransient<IndirectCircularDependencyC>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<IndirectCircularDependencyA>());

        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void NoCircularDependencySameTypeMultipleTimes()
    {
        var serviceProvider = new ServiceProviderBuilder()
            .AddTransient<NoCircularDependencySameTypeMultipleTimesA>()
            .AddTransient<NoCircularDependencySameTypeMultipleTimesB>()
            .AddTransient<NoCircularDependencySameTypeMultipleTimesC>()
            .Build();

        var resolvedService = serviceProvider.GetRequiredService<NoCircularDependencySameTypeMultipleTimesA>();
        Assert.NotNull(resolvedService);
    }

    [Fact]
    public void DependencyOnCircularDependency()
    {
        var expectedMessage = "A circular dependency was detected for the service of type " +
                              "'Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyA'." +
                              Environment.NewLine +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.DependencyOnCircularDependency -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyA -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyB -> " +
                              "Assimalign.Extensions.DependencyInjection.MockObjects.DirectCircularDependencyA";

        var serviceProvider = new ServiceProviderBuilder()
            .AddTransient<DependencyOnCircularDependency>()
            .AddTransient<DirectCircularDependencyA>()
            .AddTransient<DirectCircularDependencyB>()
            .Build();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            serviceProvider.GetRequiredService<DependencyOnCircularDependency>());

        Assert.Equal(expectedMessage, exception.Message);
    }
}
