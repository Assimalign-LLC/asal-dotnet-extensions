using System;
using System.Collections.Generic;

namespace Assimalign.Extensions.Mapping;

using Assimalign.Extensions.Mapping.Internal;

public abstract class MapperProfileBuilder : IMapperProfileBuilder
{
    private bool isBuilt;
    private IList<IMapperProfile> profiles;

    public MapperProfileBuilder()
    {
        this.profiles = new List<IMapperProfile>();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected abstract void OnBuild(IMapperProfileBuilder builder);


    /// <inheritdoc cref="IMapperProfileBuilder.CreateProfile{TTarget, TSource}(Action{IMapperActionDescriptor{TTarget, TSource}})"/>
    public IMapperProfileBuilder CreateProfile<TTarget, TSource>(Action<IMapperActionDescriptor<TTarget, TSource>> configure)
    {
        var profile = new MapperProfileDefault<TTarget, TSource>(configure);
        var descriptor = new MapperActionDescriptor<TTarget, TSource>()
        {
            MapActions = profile.MapActions
        };

        profile.Configure(descriptor);

        profiles.Add(profile);

        return this;
    }

    /// <inheritdoc cref="IMapperProfileBuilder.Build"/>
    public IEnumerable<IMapperProfile> Build()
    {
        if (!isBuilt)
        {
            OnBuild(this);
            isBuilt = true;
        }

        return profiles;
    }
}
