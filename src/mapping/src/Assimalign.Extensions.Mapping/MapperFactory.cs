using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Assimalign.Extensions.Mapping;

using Assimalign.Extensions.Mapping.Internal;

/// <summary>
/// 
/// </summary>
public sealed class MapperFactory : IMapperFactory
{
    private readonly ConcurrentDictionary<string, IMapper> mappers;

    public MapperFactory()
    {
        this.mappers = new ConcurrentDictionary<string, IMapper>(); 
    }


    /// <inheritdoc />
    public IMapper Create(string mapperName, IMapperProfileBuilder builder)
    {
        return mappers.GetOrAdd(mapperName, new Mapper(builder.Build(), new MapperOptions()));
    }

    public IMapper Create(string mapperName, IMapperProfileBuilder builder, Action<MapperOptions> configure)
    {
        return mappers.GetOrAdd(mapperName, name =>
        {
            var options = new MapperOptions();

            configure.Invoke(options);

            return new Mapper(builder.Build(), options);
        });
    }
    
    /// <inheritdoc />
    public IMapper Create(string mapperName, Action<IMapperProfileBuilder> configure)
    {
        return mappers.GetOrAdd(mapperName, name =>
        {
            var builder = new MapperProfileBuilderDefault();

            configure.Invoke(builder);

            return new Mapper(builder.Build(), new MapperOptions());
        });
    }

    public IMapper Create(string mapperName, Action<MapperOptions, IMapperProfileBuilder> configure)
    {
        return mappers.GetOrAdd(mapperName, name =>
        {
            var options = new MapperOptions();
            var builder = new MapperProfileBuilderDefault();

            configure.Invoke(options, builder);

            return new Mapper(builder.Build(), options);
        });
    }
    /
    // <inheritdoc />
    public IMapper Create(string mapperName, IEnumerable<IMapperProfile> profiles)
    {
        return mappers.GetOrAdd(mapperName, new Mapper(profiles, new MapperOptions()));
    }
}
