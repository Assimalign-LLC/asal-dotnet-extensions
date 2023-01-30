


using Assimalign.Extensions.DependencyInjection.MockObjects;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithDefaultConstructorParameters
    {
        public TypeWithDefaultConstructorParameters(
            IFakeMultipleService multipleService,
            IFakeService fakeService = null)
        {
        }

        public TypeWithDefaultConstructorParameters(
            IFactoryService factoryService)
        {
        }

        public TypeWithDefaultConstructorParameters(
            IFactoryService factoryService,
            IFakeScopedService singletonService = null)
        {
        }
    }
}
