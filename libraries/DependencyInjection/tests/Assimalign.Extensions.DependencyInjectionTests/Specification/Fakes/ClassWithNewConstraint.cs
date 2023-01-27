


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class ClassWithNewConstraint<T> : IFakeOpenGenericService<T>
        where T : new()
    {
        public T Value { get; } = new T();
    }
}
