using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup
{
    [Flags]
    internal enum CallSiteRuntimeResolverLock
    {
        Scope = 1,
        Root = 2
    }
}
