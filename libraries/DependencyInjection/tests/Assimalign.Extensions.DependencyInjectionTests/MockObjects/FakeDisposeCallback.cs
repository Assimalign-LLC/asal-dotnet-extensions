


using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class FakeDisposeCallback
    {
        public List<object> Disposed { get; } = new List<object>();
    }
}
