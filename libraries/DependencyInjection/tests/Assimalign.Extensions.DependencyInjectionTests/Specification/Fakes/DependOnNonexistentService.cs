


using Assimalign.Extensions.DependencyInjection.Specification.Fakes;

namespace Assimalign.Extensions.DependencyInjection.Tests
{
    public class DependOnNonexistentService
    {
        public DependOnNonexistentService(IFakeService nonExistentService)
        {
        }
    }
}
