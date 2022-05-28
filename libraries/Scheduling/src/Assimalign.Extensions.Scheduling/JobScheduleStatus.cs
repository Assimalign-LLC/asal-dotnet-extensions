using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public sealed class JobScheduleStatus
{
    /// <summary>
    /// 
    /// </summary>
    public DateTime LastUpdateTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime LastRunTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime NextRunTime { get; set; }
}
