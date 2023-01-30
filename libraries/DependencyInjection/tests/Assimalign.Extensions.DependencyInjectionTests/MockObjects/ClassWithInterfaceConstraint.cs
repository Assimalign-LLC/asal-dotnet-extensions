


using System.Collections;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithInterfaceConstraint<T> : IFakeOpenGenericService<T>
        where T : IEnumerable
    {
        public ClassWithInterfaceConstraint(T value) => Value = value;

        public T Value { get; }
    }
}
