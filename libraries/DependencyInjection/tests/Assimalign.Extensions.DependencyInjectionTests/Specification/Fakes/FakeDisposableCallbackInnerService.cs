


namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class FakeDisposableCallbackInnerService : FakeDisposableCallbackService, IFakeMultipleService
    {
        public FakeDisposableCallbackInnerService(FakeDisposeCallback callback) : base(callback)
        {
        }
    }
}
