


using Assimalign.Extensions.DependencyInjection.MockObjects;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithNonOverlappedConstructors
    {
        public TypeWithNonOverlappedConstructors(
            IFakeOuterService outerService)
        {
        }

        public TypeWithNonOverlappedConstructors(
            IFakeScopedService scopedService,
            IFakeService fakeService)
        {
        }

        public TypeWithNonOverlappedConstructors(
            IFakeScopedService scopedService,
            IFakeService fakeService,
            IFakeMultipleService multipleService)
        {
        }
    }
}
