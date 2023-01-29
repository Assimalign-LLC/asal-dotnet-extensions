using System;

namespace Assimalign.Extensions.DependencyInjection.Internal;

internal abstract class CompiledServiceProviderEngine : ServiceProviderEngine
{
#if IL_EMIT
        public ILEmitResolverBuilder ResolverBuilder { get; }
#else
    public CallSiteExpressionResolverBuilderVisitor ResolverBuilder { get; }
#endif
    
    public CompiledServiceProviderEngine(ServiceProvider provider)
    {
        ResolverBuilder = new(provider);
    }

    public override Func<ServiceProviderEngineScope, object> RealizeService(CallSiteService callSite) => ResolverBuilder.Build(callSite);
}
