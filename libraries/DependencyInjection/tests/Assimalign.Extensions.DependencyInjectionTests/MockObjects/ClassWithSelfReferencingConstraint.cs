


using System;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithSelfReferencingConstraint<T> : IFakeOpenGenericService<T>
        where T : IComparable<T>
    {
        public ClassWithSelfReferencingConstraint(T value) => Value = value;

        public T Value { get; }
    }
}
