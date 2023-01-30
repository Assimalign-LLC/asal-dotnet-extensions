


using Assimalign.Extensions.DependencyInjection.MockObjects;

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
