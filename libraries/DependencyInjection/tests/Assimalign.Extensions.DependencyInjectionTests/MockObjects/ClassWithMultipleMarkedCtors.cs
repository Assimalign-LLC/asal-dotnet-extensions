


using Assimalign.Extensions.DependencyInjection.Utilities;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithMultipleMarkedCtors
    {
        [ActivatorUtilitiesConstructor]
        public ClassWithMultipleMarkedCtors(string data)
        {
        }

        [ActivatorUtilitiesConstructor]
        public ClassWithMultipleMarkedCtors(IFakeService service, string data)
        {
        }
    }
}
