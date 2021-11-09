using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup
{
    internal struct CallSiteRuntimeResolverContext
    {
        public ServiceProviderEngineScope Scope { get; set; }

        public CallSiteRuntimeResolverLock AcquiredLocks { get; set; }
    }
}
