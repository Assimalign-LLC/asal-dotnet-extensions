using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assimalign.Extensions.Scheduling;

public sealed partial class JobSchedule
{
    /// <summary>
    /// Creates a job schedule to be invoked at the specified time: <paramref name="ticks"/>.
    /// </summary>
    /// <param name="ticks">The time in which to run the schedule.</param>
    /// <returns></returns>
    public static IJobSchedule Daily(long ticks)
    {
        return Daily(new TimeSpan(ticks));
    }
    public static IJobSchedule Daily(string timeSpan)
    {
        return Daily(TimeSpan.Parse(timeSpan));
    }
    public static IJobSchedule Daily(TimeSpan timeSpan)
    {
        if (timeSpan > TimeSpan.FromDays(1))
        {
            throw new ArgumentOutOfRangeException("Daily Job Schedule cannot have a timespan greater than one day.");
        }

        var scheduler = new JobScheduleDefault()
        {
            StartTime = DateTime.Today.TimeOfDay > timeSpan ?
                timeSpan :
                TimeSpan.FromTicks(TimeSpan.TicksPerDay - DateTime.Now.TimeOfDay.Ticks).Add(timeSpan),
            Interval = TimeSpan.FromDays(1)
        };

        return scheduler;
    }
    public static IJobSchedule Weekly(DayOfWeek dayOfWeek)
    {
        throw new NotImplementedException();
    }
    public static IJobSchedule Monthly(int day)
    {
        throw new NotImplementedException();
    }   
}