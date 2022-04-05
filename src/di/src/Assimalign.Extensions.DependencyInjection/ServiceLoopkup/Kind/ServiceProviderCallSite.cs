using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup.Kind
{
    internal sealed class ServiceProviderCallSite : CallSiteService
    {
        public ServiceProviderCallSite() : base(CallSiteResultCache.None)
        {
        }

        public override Type ServiceType { get; } = typeof(IServiceProvider);
        public override Type ImplementationType { get; } = typeof(ServiceProvider);
        public override CallSiteKind Kind { get; } = CallSiteKind.ServiceProvider;
    }
}
