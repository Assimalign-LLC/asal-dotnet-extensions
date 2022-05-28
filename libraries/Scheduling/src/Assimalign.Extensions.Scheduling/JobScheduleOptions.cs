using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public sealed class JobScheduleOptions
{
    /// <summary>
    /// The interval in which to check each job schedule. The default is 2 seconds.
    /// </summary>
    public TimeSpan ScheduleCheckInterval { get; set; } = TimeSpan.FromSeconds(2);
    public bool RunOnStartup { get; set; }
    public bool AdjustForDST { get; set; }
    public static JobScheduleOptions Default => throw new NotImplementedException();
}
