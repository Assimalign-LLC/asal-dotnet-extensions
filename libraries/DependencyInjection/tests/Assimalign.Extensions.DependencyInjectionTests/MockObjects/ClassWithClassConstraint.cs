


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithClassConstraint<T> : IFakeOpenGenericService<T>
        where T : class
    {
        public T Value { get; }
    }
}
