using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.Internal;

internal sealed class ILEmitServiceProviderEngine : ServiceProviderEngine
{
    private readonly ILEmitResolverBuilder _expressionResolverBuilder;
    public ILEmitServiceProviderEngine(ServiceProvider serviceProvider)
    {
        _expressionResolverBuilder = new ILEmitResolverBuilder(serviceProvider);
    }

    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite)
    {
        return _expressionResolverBuilder.Build(callSite);
    }
}
