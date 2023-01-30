


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithAbstractClassConstraint<T> : IFakeOpenGenericService<T>
        where T : AbstractClass
    {
        public ClassWithAbstractClassConstraint(T value) => Value = value;

        public T Value { get; }
    }
}
