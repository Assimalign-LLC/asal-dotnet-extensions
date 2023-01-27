using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection;

public sealed class ServiceProviderFactoryBuilder
{

    private readonly IDictionary<object, List<Action<IServiceCollection>>> serviceActions;

    public ServiceProviderFactoryBuilder()
    {
        this.serviceActions = new Dictionary<object, List<Action<IServiceCollection>>>();
    }


    public ServiceProviderFactoryBuilder AddServiceProvider<TKey>(TKey key, Action<IServiceCollection> services)
        where TKey : IComparable
    {
        if (serviceActions.TryGetValue(key, out var actions))
        {
            actions.Add(services);
        }
        else
        {
            serviceActions[key] = new List<Action<IServiceCollection>>()
            {
                services
            };
        }

        return this;
    }


    public IServiceProviderFactory Build()
    {

        var providers = new ConcurrentDictionary<object, Func<IServiceProvider>>();
        var factory = ServiceProviderFactory.New(providers);

        foreach (var (key, value) in serviceActions)
        {
            var serviceCollection = new ServiceCollection();

            foreach (var action in value)
            {
                action.Invoke(serviceCollection);
            }

            serviceCollection.AddSingleton<IServiceProviderFactory>(factory);

            providers[key] = () => serviceCollection.BuildServiceProvider();
        }

        return factory;
    }
}
