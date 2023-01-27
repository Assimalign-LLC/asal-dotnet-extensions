


using System.Collections.Generic;
using Assimalign.Extensions.DependencyInjection.Specification.Fakes;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithUnresolvableEnumerableConstructors
    {
        public TypeWithUnresolvableEnumerableConstructors(IFakeService fakeService)
        {
        }

        public TypeWithUnresolvableEnumerableConstructors(IEnumerable<IFakeService> fakeService)
        {
        }

        public TypeWithUnresolvableEnumerableConstructors(IFactoryService factoryService)
        {
        }
    }
}
