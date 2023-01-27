using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.LifeCycle.Internal;

internal struct ValueState<T> : IState<T>
    where T : struct
{
    private T value;

    public ValueState(ref T value)
    {
        this.value = value;
    }

    public T GetValue() => this.value;
    public void SetValue(T value)
    {
    
        throw new NotImplementedException();
    }
}
