using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public sealed class JobScheduleOptions
{
    public bool RunOnStartup { get; set; }
    public static JobScheduleOptions Default => throw new NotImplementedException();
}
