


using Assimalign.Extensions.DependencyInjection.Specification.Fakes;

namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithParameterizedAndNullaryConstructor
    {
        public TypeWithParameterizedAndNullaryConstructor()
            : this(new FakeService())
        {

        }

        public TypeWithParameterizedAndNullaryConstructor(IFakeService fakeService)
        {
        }
    }
}
