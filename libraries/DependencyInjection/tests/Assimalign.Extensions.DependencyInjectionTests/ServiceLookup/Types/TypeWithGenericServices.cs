


using Assimalign.Extensions.DependencyInjection.Specification.Fakes;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithGenericServices
    {
        public TypeWithGenericServices(
            IFakeService fakeService,
            IFakeOpenGenericService<IFakeService> logger)
        {
        }

        public TypeWithGenericServices(
           IFakeMultipleService multipleService,
           IFakeService fakeService)
        {
        }

        public TypeWithGenericServices(
            IFakeService fakeService,
            IFactoryService factoryService,
            IFakeOpenGenericService<IFakeService> logger)
        {
        }
    }
}
