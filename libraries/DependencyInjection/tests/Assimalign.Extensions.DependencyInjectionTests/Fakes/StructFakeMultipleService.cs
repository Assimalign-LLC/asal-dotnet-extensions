


using Assimalign.Extensions.DependencyInjection.Specification.Fakes;

namespace Assimalign.Extensions.DependencyInjection.Fakes
{
    public struct StructFakeMultipleService : IFakeMultipleService
    {
        public StructFakeMultipleService(IFakeService service, StructService direct)
        {
        }
    }
}
