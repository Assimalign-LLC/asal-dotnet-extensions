


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class ClassWithStructConstraint<T> : IFakeOpenGenericService<T>
        where T : struct
    {
        public T Value { get; }
    }
}
