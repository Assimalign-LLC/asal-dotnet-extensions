using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.Internal;

internal sealed class RuntimeServiceProviderEngine : ServiceProviderEngine
{
    public static RuntimeServiceProviderEngine Instance { get; } = new RuntimeServiceProviderEngine();

    private RuntimeServiceProviderEngine() { }

    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite)
    {
        return scope =>
        {
            return CallSiteRuntimeResolverVisitor.Instance.Resolve(callSite, scope);
        };
    }
}
