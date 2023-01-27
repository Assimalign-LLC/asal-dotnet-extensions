


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class ClassWithAbstractClassConstraint<T> : IFakeOpenGenericService<T>
        where T : AbstractClass
    {
        public ClassWithAbstractClassConstraint(T value) => Value = value;

        public T Value { get; }
    }
}
