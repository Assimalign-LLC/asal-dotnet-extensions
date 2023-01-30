


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class FakeDisposableCallbackInnerService : FakeDisposableCallbackService, IFakeMultipleService
    {
        public FakeDisposableCallbackInnerService(FakeDisposeCallback callback) : base(callback)
        {
        }
    }
}
