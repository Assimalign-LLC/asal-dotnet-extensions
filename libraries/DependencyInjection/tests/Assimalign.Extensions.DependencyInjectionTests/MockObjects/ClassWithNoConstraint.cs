


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithNoConstraints<T> : IFakeOpenGenericService<T>
    {
        public T Value { get; }
    }
}
