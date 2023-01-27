


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class ClassWithClassConstraint<T> : IFakeOpenGenericService<T>
        where T : class
    {
        public T Value { get; }
    }
}
