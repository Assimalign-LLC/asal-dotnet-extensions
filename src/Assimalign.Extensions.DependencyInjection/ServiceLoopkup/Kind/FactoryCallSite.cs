using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup.Kind
{
    internal sealed class FactoryCallSite : CallSiteService
    {
        public Func<IServiceProvider, object> Factory { get; }

        public FactoryCallSite(CallSiteResultCache cache, Type serviceType, Func<IServiceProvider, object> factory) : base(cache)
        {
            Factory = factory;
            ServiceType = serviceType;
        }

        public override Type ServiceType { get; }
        public override Type ImplementationType => null;

        public override CallSiteKind Kind { get; } = CallSiteKind.Factory;
    }
}
