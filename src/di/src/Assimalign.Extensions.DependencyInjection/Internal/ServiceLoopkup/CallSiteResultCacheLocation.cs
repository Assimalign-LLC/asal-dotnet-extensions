using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup
{
    internal enum CallSiteResultCacheLocation
    {
        Root,
        Scope,
        Dispose,
        None
    }
}
