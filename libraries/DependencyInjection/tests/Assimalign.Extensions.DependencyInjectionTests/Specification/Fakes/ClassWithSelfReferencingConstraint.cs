


using System;

namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class ClassWithSelfReferencingConstraint<T> : IFakeOpenGenericService<T>
        where T : IComparable<T>
    {
        public ClassWithSelfReferencingConstraint(T value) => Value = value;

        public T Value { get; }
    }
}
