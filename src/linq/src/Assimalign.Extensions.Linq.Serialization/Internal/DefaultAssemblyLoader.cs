using System;
using System.Collections.Generic;
using System.Reflection;


namespace Assimalign.Extensions.Linq.Serialization.Internal;

internal class DefaultAssemblyLoader : ILinqExpressionAssemblyLoader
{
    public IEnumerable<Assembly> GetAssemblies()
    {
        return AppDomain.CurrentDomain.GetAssemblies();
    }
}
