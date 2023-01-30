


namespace Assimalign.Extensions.DependencyInjection.MockObjects;

public class TransientFactoryService : IFactoryService
{
    public IFakeService FakeService { get; set; }

    public int Value { get; set; }
}
