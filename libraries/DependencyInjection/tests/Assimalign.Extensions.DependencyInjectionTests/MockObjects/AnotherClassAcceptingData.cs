


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class AnotherClassAcceptingData
    {
        public AnotherClassAcceptingData(IFakeService fakeService, string one, string two)
        {
            FakeService = fakeService;
            One = one;
            Two = two;
        }

        public IFakeService FakeService { get; }

        public string One { get; }

        public string Two { get; }
    }
}
