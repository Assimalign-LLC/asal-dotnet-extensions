using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;


public sealed class JobScheduleBuilder
{

    public JobScheduleBuilder()
    {

    }

    internal IList<Action> OnStartActions { get; }
    internal IList<Action> OnStopActions { get; }
    internal IList<Action> OnRunActions { get; }

    public JobScheduleBuilder AddOnStartAction(Action action)
    {

    }
}
