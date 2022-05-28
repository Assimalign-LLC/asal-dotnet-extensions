using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public abstract class JobSchedule : IJobSchedule
{
    private DateTime nextRunTime;
    private DateTime lastRunTime;

    public JobSchedule()
    {
        this.ScheduleStatus = new JobScheduleStatus();
    }


    /// <inheritdoc />
    public virtual string Id { get; } = Guid.NewGuid().ToString("N");
    /// <inheritdoc />
    public IEnumerable<IJob> Jobs => throw new NotImplementedException();

    public JobScheduleStatus ScheduleStatus { get; set; }

    public virtual void OnStart() { }

    /// <inheritdoc />
    public abstract void Run();


    public virtual void OnComplete() { }

    public abstract DateTime GetNextOccurrence(DateTime now);

    public abstract IEnumerable<DateTime> GetNextOccurrences(int count, DateTime? now = null);
}
