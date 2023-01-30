


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public interface IFakeEveryService :
        IFakeService,
        IFakeMultipleService,
        IFakeScopedService,
        IFakeServiceInstance,
        IFakeSingletonService,
        IFakeOpenGenericService<PocoClass>
    {
    }
}
