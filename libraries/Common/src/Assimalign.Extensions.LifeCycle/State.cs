using Assimalign.Extensions.LifeCycle.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.LifeCycle;


public class StaticStateStore : IStateStore
{
    public static void UseEffect(Action action)
    {
        throw new NotImplementedException();
    }

    public static void UseEffect(Action action, params object[] dependencies)
    {
        throw new NotImplementedException();
    }

    public static Tuple<T, StateSetter<T>> UseState<T>()
    {
        throw new NotImplementedException();
    }

    public static Tuple<T, StateSetter<T>> UseState<T>(T state)
    {
        throw new NotImplementedException();
    }
}

public sealed partial class State : IStateStoreFactory
{
    public IStateStore Create(StateStoreType stateStoreType)
    {
        return stateStoreType switch
        {
            StateStoreType.Default  => new DefaultStateStore(),
            StateStoreType.Global   => GlobalStateStore.New()
        };
    }
}
