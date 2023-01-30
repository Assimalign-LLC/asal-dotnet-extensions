


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithNewConstraint<T> : IFakeOpenGenericService<T>
        where T : new()
    {
        public T Value { get; } = new T();
    }
}
