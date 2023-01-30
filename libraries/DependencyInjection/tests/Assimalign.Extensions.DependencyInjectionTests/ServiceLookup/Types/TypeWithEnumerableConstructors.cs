


using System.Collections.Generic;
using Assimalign.Extensions.DependencyInjection.MockObjects;

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
