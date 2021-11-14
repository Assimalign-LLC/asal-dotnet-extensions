using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup
{
    using Assimalign.Extensions.DependencyInjection.ServiceLoopkup.ILEmit;

    internal abstract class CompiledServiceProviderEngine : ServiceProviderEngine
    {

        public ILEmitResolverBuilder ResolverBuilder { get; }


        public CompiledServiceProviderEngine(ServiceProvider provider)
        {
            ResolverBuilder = new(provider);
        }

        public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite) => ResolverBuilder.Build(callSite);
    }
}
