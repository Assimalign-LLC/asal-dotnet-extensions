


using System;

namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public class FakeDisposableCallbackService: IDisposable
    {
        private static int _globalId;
        private readonly int _id;
        private readonly FakeDisposeCallback _callback;

        public FakeDisposableCallbackService(FakeDisposeCallback callback)
        {
            _id = _globalId++;
            _callback = callback;
        }

        public void Dispose()
        {
            _callback.Disposed.Add(this);
        }

        public override string ToString()
        {
            return _id.ToString();
        }
    }
}
