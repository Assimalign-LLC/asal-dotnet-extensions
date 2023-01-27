


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
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
