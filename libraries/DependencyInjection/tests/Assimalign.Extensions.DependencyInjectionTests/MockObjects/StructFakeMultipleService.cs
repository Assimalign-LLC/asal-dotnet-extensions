


using Assimalign.Extensions.DependencyInjection.MockObjects;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public struct StructFakeMultipleService : IFakeMultipleService
    {
        public StructFakeMultipleService(IFakeService service, StructService direct)
        {
        }
    }
}
