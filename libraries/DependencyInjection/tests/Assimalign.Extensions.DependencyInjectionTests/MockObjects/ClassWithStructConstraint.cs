


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class ClassWithStructConstraint<T> : IFakeOpenGenericService<T>
        where T : struct
    {
        public T Value { get; }
    }
}
