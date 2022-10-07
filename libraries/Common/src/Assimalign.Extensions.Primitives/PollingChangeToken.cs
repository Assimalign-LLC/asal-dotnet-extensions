using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Primitives;

public class PollingChangeToken : IChangeToken
{
    public bool HasChanged => throw new NotImplementedException();

    public bool ActiveChangeCallbacks => throw new NotImplementedException();

    public TimeSpan PollingInterval { get; }

    public IDisposable RegisterChangeCallback(Action<object> callback, object state)
    {
        throw new NotImplementedException();
    }
}
