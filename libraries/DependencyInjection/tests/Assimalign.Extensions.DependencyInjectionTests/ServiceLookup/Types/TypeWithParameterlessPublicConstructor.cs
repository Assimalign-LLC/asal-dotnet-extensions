


namespace Assimalign.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithParameterlessPublicConstructor
    {
        public TypeWithParameterlessPublicConstructor()
            : this("some name")
        {
        }

        protected TypeWithParameterlessPublicConstructor(string name)
        {
        }
    }
}
