using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public sealed class CrontabSchedule : IJobSchedule
{
    private readonly Crontab crontab;

    private Timer timer;

    public CrontabSchedule()
    {

    }
    public string Id => throw new NotImplementedException();

    public Timer Timer => this.timer;

    public DateTime StartDate => throw new NotImplementedException();

    public TimeSpan StartTime => throw new NotImplementedException();

    public TimeSpan Interval => throw new NotImplementedException();

    public DateTime LastRunTime => throw new NotImplementedException();

    public DateTime NextRunTime => throw new NotImplementedException();

    public IEnumerable<IJob> Jobs => throw new NotImplementedException();

    public JobScheduleStatus Status => throw new NotImplementedException();

    public IJobSchedule AttachJob(IJob job)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public void OnComplete(IJobContext context)
    {
        timer = new Timer(
            new TimerCallback(Run),
            context,
            crontab.GetTimeSpan(),
            Timeout.InfiniteTimeSpan); // <- Since CronSchedule can have varied occurrences, only want the timer to callback to fire off once.
    }

    public void OnRun(IJobContext context)
    {
        var auto = new AutoResetEvent(false);

        auto.WaitOne();
        throw new NotImplementedException();
    }

    public void OnStart()
    {
        throw new NotImplementedException();
    }

    private void Run(object state)
    {

    }
}
