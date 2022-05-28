using System;
using Xunit;


namespace Assimalign.Extensions.ObjectPool.Tests;

public class DefaultObjectPoolProviderTest
{

    [Fact]
    public void DefaultObjectPoolProvider_CreateForObject_DefaultObjectPoolReturned()
    {
        // Arrange
        var provider = new ObjectPoolProviderDefault();

        // Act
        var pool = provider.Create<object>();

        // Assert
        Assert.IsType<ObjectPoolDefault<object>>(pool);
    }

    [Fact]
    public void DefaultObjectPoolProvider_CreateForIDisposable_DisposableObjectPoolReturned()
    {
        // Arrange
        var provider = new ObjectPoolProviderDefault();

        // Act
        var pool = provider.Create<DisposableObject>();

        // Assert
        Assert.IsType<ObjectPoolDisposable<DisposableObject>>(pool);
    }

    private class DisposableObject : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose() => IsDisposed = true;
    }
}
