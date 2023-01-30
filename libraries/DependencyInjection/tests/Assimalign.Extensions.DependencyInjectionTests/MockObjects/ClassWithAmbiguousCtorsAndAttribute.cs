


using Assimalign.Extensions.DependencyInjection.Utilities;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithAmbiguousCtorsAndAttribute
    {
        public ClassWithAmbiguousCtorsAndAttribute(string data)
        {
            CtorUsed = "string";
        }

        [ActivatorUtilitiesConstructor]
        public ClassWithAmbiguousCtorsAndAttribute(IFakeService service, string data)
        {
            CtorUsed = "IFakeService, string";
        }

        public ClassWithAmbiguousCtorsAndAttribute(IFakeService service, IFakeOuterService service2, string data)
        {
            CtorUsed = "IFakeService, IFakeService, string";
        }

        public string CtorUsed { get; set; }
    }
}
