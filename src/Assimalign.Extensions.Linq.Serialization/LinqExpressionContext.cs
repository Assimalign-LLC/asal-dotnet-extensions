


using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assimalign.Extensions.Linq.Serialization;

using Assimalign.Extensions.Linq.Serialization.Internals;

public class LinqExpressionContext : LinqExpressionContextBase
{
    private readonly ILinqExpressionAssemblyLoader _assemblyLoader;

    public LinqExpressionContext()
        : this(new DefaultAssemblyLoader()) { }

    public LinqExpressionContext(ILinqExpressionAssemblyLoader assemblyLoader)
    {
        _assemblyLoader = assemblyLoader 
            ?? throw new ArgumentNullException(nameof(assemblyLoader));
    }

    protected override IEnumerable<Assembly> GetAssemblies()
    {
        return _assemblyLoader.GetAssemblies();
    }
}
