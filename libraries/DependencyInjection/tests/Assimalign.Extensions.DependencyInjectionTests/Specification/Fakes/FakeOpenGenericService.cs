


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class FakeOpenGenericService<TVal> : IFakeOpenGenericService<TVal>
    {
        public FakeOpenGenericService(TVal value)
        {
            Value = value;
        }

        public TVal Value { get; }
    }
}
