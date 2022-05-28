using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assimalign.Extensions.Scheduling;

public sealed class JobSchedule : IJobSchedule
{
    private string id;
    private readonly IList<IJob> jobs;


    private JobSchedule()
    {
        OnStart();
    }

    public string Id { get; } = Guid.NewGuid().ToString("N");
    public Timer Timer { get; private set; }
    public DateTime StartDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan Interval { get; init; }
    public DateTime LastRunTime { get; private set; }
    public DateTime NextRunTime
    {
        get
        {
            var now = DateTime.Now;
            var next = StartDate;

            while (next < now)
            {
                next = next.Add(Interval);
            }

            return next;
        }
    }
    public IEnumerable<IJob> Jobs => this.jobs;
    public JobScheduleStatus Status { get; private set; }
    public IJobSchedule AttachJob(IJob job)
    {
        this.jobs.Add(job);
        return this;
    }

    Func<IJobContext> GetContext { get; } = () => default;

    public void OnStart()
    {
        if (Timer is null)
        {
            return;
        }

        Timer = new Timer(new TimerCallback(Run), default, StartTime, Interval);
    }
    public void OnRun()
    {
        // Let's prevent 
        if (NextRunTime < DateTime.Now)
        {
            throw new InvalidOperationException("");
        }

        Status = JobScheduleStatus.Running;

        var context = GetContext.Invoke();

        foreach (var job in this.Jobs)
        {
            job.Invoke(context);
        }

        OnComplete(context);
    }
    public void OnComplete(IJobContext context)
    {
        Status = JobScheduleStatus.Waiting;
        LastRunTime = DateTime.Now;
    }

    private void Run(object state)
    {
        if (state is not IJobContext context)
        {
            throw new ArgumentException("");
        }

        OnRun();
    }
    public void Dispose()
    {
        Timer.Dispose();
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

        IJobSchedule scheduler = new JobSchedule()
        {
            StartTime = DateTime.Today.TimeOfDay > timeSpan ?
                timeSpan :
                TimeSpan.FromTicks(TimeSpan.TicksPerDay - DateTime.Now.TimeOfDay.Ticks).Add(timeSpan),
            Interval = TimeSpan.FromDays(1)
        };

        return scheduler;
    }
    public static IJobSchedule Crontab(string expression)
    {
        throw new NotImplementedException();
    }
}