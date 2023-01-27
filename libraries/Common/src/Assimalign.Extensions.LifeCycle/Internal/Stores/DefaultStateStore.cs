using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.LifeCycle.Internal;

internal class DefaultStateStore : IStateStore
{
    public void UseEffect(Action action)
    {
        throw new NotImplementedException();
    }

    public void UseEffect(Action action, params object[] dependencies)
    {
        throw new NotImplementedException();
    }

    public Tuple<T, StateSetter<T>> UseState<T>()
    {
        throw new NotImplementedException();
    }

    public Tuple<T, StateSetter<T>> UseState<T>(T state)
    {
        throw new NotImplementedException();
    }
}
