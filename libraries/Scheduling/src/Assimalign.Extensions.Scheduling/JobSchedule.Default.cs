using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assimalign.Extensions.Scheduling;

public sealed partial class JobSchedule : IJobSchedule
{
    private readonly IList<IJob> jobs;

    private readonly Func<IJobContext> invoke;

    private readonly IList<Action> onStartActions = new List<Action>();
    private readonly IList<Action> onStopActions = new List<Action>();


    private JobScheduleStatus status;

    public JobSchedule()
    {
        this.Id = Guid.NewGuid().ToString("N");
    }

    public string Id { get; }
    public Timer Timer { get; private set; }
    public DateTime StartDate { get; init; }
    public TimeSpan StartTime { get; init; }
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
    public JobScheduleStatus Status => this.status;
    public IJobSchedule AttachJob(IJob job)
    {
        this.jobs.Add(job);
        return this;
    }

    public void OnStart()
    {
        Status = JobScheduleStatus.Starting;
        // Check if timer has already been set
        // If so, return back to the caller
        if (Timer is not null)
        {
            return;
        }
        foreach (var action in onStartActions)
        {
            action();
        }
        Timer = new Timer(new TimerCallback(Run), GetContext.Invoke(), StartTime, Interval);
    }
    public void OnStop()
    {
        foreach (var action in onStartActions)
        {
            action.Invoke();
        }
    }

    private void Run(object state)
    {
        if (state is not IJobContext context)
        {
            throw new ArgumentException("");
        }

        // Let's prevent 
        if (NextRunTime < DateTime.Now)
        {
            throw new InvalidOperationException("");
        }

        this.status = JobScheduleStatus.Running;

        foreach (var action in onRunActions)
        {
            action.Invoke(context);
        }

        var tasks = new List<Task>();

        foreach (var job in this.Jobs)
        {
            tasks.Add(job.InvokeAsync(context, default));
        }

        var all = Task.WhenAll(tasks);

        all.Wait();

        this.status = JobScheduleStatus.Waiting;
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }
    public ValueTask DisposeAsync()
    {
        // max disposable time is 3 seconds
        var maxDisposableTime = TimeSpan.FromSeconds(3);
        var timeout = DateTime.UtcNow.Add(maxDisposableTime);

        while (true)
        {
            if (Status != JobScheduleStatus.Running)
            {
                OnStop();
                Timer.Dispose();
                break;
            }
            if (timeout > DateTime.UtcNow)
            {
                break;
            }
        }
        return ValueTask.CompletedTask;
    }
}