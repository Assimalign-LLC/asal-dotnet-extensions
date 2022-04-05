using System;

namespace Assimalign.ComponentModel.Mapping.Internal;

internal sealed class MapperAction<TTarget, TSource> : IMapperAction
{
    private readonly Action<TTarget, TSource> configure;

    public MapperAction(Action<TTarget, TSource> configure) => this.configure = configure;
    public int Id => this.GetHashCode();
    public void Invoke(IMapperContext context)
    {
        if (context.Target is TTarget target && context.Source is TSource source)
        {
            configure.Invoke(target, source);
        }
    }
}