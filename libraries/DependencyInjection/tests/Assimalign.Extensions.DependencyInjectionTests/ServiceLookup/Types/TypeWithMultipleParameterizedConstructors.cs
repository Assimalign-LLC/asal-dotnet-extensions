


using Assimalign.Extensions.DependencyInjection.Specification.Fakes;

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
