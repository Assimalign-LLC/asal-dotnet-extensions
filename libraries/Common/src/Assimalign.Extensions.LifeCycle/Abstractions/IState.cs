using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Assimalign.Extensions.LifeCycle;

public interface IState<T>
{
    T GetValue();

    void SetValue(T value);

}
