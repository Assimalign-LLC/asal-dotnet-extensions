using System;

namespace Assimalign.Extensions.DependencyInjection.Internal;

internal abstract class ServiceProviderEngine
{
    public abstract Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite);
}
