using System;

namespace Assimalign.Extensions.LifeCycle;

public interface IStateStore
{
    Tuple<T, StateSetter<T>> UseState<T>();
    Tuple<T, StateSetter<T>> UseState<T>(T state);
    void UseEffect(Action action);
    void UseEffect(Action action, params object[] dependencies);
}
