


using System.Collections.Generic;
using Assimalign.Extensions.DependencyInjection.Specification.Fakes;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithEnumerableConstructors
    {
        public TypeWithEnumerableConstructors(
            IEnumerable<IFakeService> fakeServices)
        {
        }

        public TypeWithEnumerableConstructors(
            IEnumerable<IFakeService> fakeServices,
            IEnumerable<IFactoryService> factoryServices)
        {
        }
    }
}
