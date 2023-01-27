


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public interface IFakeOpenGenericService<out TValue>
    {
        TValue Value { get; }
    }
}
