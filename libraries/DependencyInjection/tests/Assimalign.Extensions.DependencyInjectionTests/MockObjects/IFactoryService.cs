


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public interface IFactoryService
    {
        IFakeService FakeService { get; }

        int Value { get; }
    }
}
