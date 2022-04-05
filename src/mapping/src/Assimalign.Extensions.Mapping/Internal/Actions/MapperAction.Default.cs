using System;

namespace Assimalign.ComponentModel.Mapping.Internal;

internal sealed class MapperAction : IMapperAction
{
    private readonly Action<IMapperContext> configure;
    public MapperAction(Action<IMapperContext> configure) => this.configure = configure;
    public int Id => this.GetHashCode();
    public void Invoke(IMapperContext context) => configure.Invoke(context);
}