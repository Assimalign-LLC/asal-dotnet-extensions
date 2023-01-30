


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public interface IFakeOpenGenericService<out TValue>
    {
        TValue Value { get; }
    }
}
