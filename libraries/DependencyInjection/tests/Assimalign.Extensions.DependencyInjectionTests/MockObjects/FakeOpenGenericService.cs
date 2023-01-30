


namespace Assimalign.Extensions.DependencyInjection.MockObjects
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
