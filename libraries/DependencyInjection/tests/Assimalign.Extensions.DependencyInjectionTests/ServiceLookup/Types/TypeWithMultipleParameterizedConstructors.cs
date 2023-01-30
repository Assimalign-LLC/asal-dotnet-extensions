


using Assimalign.Extensions.DependencyInjection.MockObjects;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithMultipleParameterizedConstructors
    {
        public TypeWithMultipleParameterizedConstructors(IFactoryService factoryService)
        {
        }

        public TypeWithMultipleParameterizedConstructors(IFakeService fakeService)
        {
        }
    }
}
