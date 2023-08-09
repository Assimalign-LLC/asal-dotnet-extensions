using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

using Assimalign.Extensions.Mapping;

public static class MappingServiceProviderBuilderExtensions
{
    private static readonly Dictionary<string, IList<Action<MapperFactoryBuilder>>> actions = new();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMapperProfile"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapping<TMapperProfile>(this IServiceProviderBuilder builder)
        where TMapperProfile : IMapperProfile, new()
    {
        return builder.AddSingleton<IMapper>(serviceProvider =>
        {
            var factoryBuilder = new MapperFactoryBuilder();

            factoryBuilder.AddMapper()
            .AddMapper()
            var factory = serviceProvider.GetService<IMapperFactory>();

            factory.

        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMapperProfile"></typeparam>
    /// <param name="services"></param>
    /// <param name="mapperName"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapping<TMapperProfile>(this IServiceCollection services, string mapperName)
        where TMapperProfile : IMapperProfile, new()
    {
        services.TryAddSingleton<IMapperFactory, MapperFactory>();
        return services.AddSingleton<IMapper>(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IMapperFactory>();

            factory.

           

        });

        return services;
    }
}
