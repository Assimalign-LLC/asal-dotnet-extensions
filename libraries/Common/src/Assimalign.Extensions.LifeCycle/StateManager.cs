using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.LifeCycle;

public static class StateManager
{


    public static Tuple<T, Action<T>> UseState<T>(T initialize)
        where T : class
    {

    }


    public static void UseEffect(Action action, params object[] dependencyArray)
    {

    }

}
