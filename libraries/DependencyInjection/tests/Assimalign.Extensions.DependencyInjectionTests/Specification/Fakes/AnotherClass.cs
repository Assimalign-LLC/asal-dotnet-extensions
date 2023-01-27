


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class AnotherClass
    {
        public AnotherClass(IFakeService fakeService)
        {
            FakeService = fakeService;
        }

        public IFakeService FakeService { get; }
    }
}
