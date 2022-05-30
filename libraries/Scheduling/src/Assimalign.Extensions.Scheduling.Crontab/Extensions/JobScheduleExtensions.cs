using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

/// <summary>
/// 
/// </summary>
public static class JobScheduleExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IJobSchedule FromCrontab(string expression)
    {
        var schedule = CrontabSchedule.Parse("");

  
        return new JobSchedule.JobScheduleDefault()
        {

        };
    }
}
