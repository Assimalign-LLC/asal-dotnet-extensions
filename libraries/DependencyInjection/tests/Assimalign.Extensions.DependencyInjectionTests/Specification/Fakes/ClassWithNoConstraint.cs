


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class ClassWithNoConstraints<T> : IFakeOpenGenericService<T>
    {
        public T Value { get; }
    }
}
