


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public interface IFactoryService
    {
        IFakeService FakeService { get; }

        int Value { get; }
    }
}
