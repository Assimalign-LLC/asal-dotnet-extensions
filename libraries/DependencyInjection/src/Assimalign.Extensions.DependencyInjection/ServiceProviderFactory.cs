using System;
using System.Collections.Concurrent;

namespace Assimalign.Extensions.DependencyInjection;


/// <summary>
/// 
/// </summary>
/// <remarks>
/// Avoid using this factory implementation within core application code. This is meant to be a way of managing service containers
/// from a parent level. For example 
/// </remarks>
public sealed class ServiceProviderFactory : IServiceProviderFactory
{
    private static IServiceProviderFactory factory;
    private ConcurrentDictionary<object, Func<IServiceProvider>> providers;

    private ServiceProviderFactory(ConcurrentDictionary<object, Func<IServiceProvider>> providers)
    {
        this.providers = providers;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public IServiceProvider Create<TKey>(TKey key) where TKey : IComparable
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (providers.TryGetValue(key, out var method))
        {
            return method.Invoke();
        }
        throw new NotImplementedException();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceProviderFactory Create(Action<ServiceProviderFactoryBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new ServiceProviderFactoryBuilder();

        configure.Invoke(builder);

        return builder.Build();
    }
    internal static IServiceProviderFactory New(ConcurrentDictionary<object, Func<IServiceProvider>> providers)
    {
        factory ??= new ServiceProviderFactory(providers);

        return factory;
    }

    public IServiceProvider Create(IServiceCollection services)
    {
        Func<IServiceProvider> func = () => services.BuildServiceProvider();

        
    }
}
