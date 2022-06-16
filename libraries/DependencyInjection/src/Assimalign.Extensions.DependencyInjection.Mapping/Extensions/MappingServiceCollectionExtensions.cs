using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection;

using Assimalign.Extensions.Mapping;

public static class MappingServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMapperProfile"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMapping<TMapperProfile>(this IServiceCollection services)
        where TMapperProfile : IMapperProfile
    {
        return services.AddSingleton<IMapper>(serviceProvider =>
        {
            var factory = serviceProvider.GetService<IMapperFactory>();

            factory

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
        where TMapperProfile : IMapperProfile
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
