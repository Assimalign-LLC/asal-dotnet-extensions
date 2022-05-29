using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assimalign.Extensions.Scheduling;

public sealed partial class JobSchedule
{
    internal sealed class JobScheduleDefault : IJobSchedule
    {
        private string id;
        private readonly IList<IJob> jobs;

        private readonly Func<IJobContext> invoke;
        private readonly IList<Action> onStartActions = new List<Action>();
        private readonly IList<Action> onStopActions = new List<Action>();
        private readonly IList<Action<IJobContext>> onRunActions = new List<Action<IJobContext>>();
        private readonly IList<Action<IJobContext>> onCompleteActions = new List<Action<IJobContext>>();

        public JobScheduleDefault()
        {

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
        public void OnRun(IJobContext context)
        {
            // Let's prevent 
            if (NextRunTime < DateTime.Now)
            {
                throw new InvalidOperationException("");
            }

            Status = JobScheduleStatus.Running;

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

            OnComplete(context);
        }
        public void OnComplete(IJobContext context)
        {
            LastRunTime = DateTime.Now;

            foreach (var action in onCompleteActions)
            {
                action.Invoke(context);
            }

            Status = JobScheduleStatus.Waiting;
        }

        private void Run(object state)
        {
            if (state is not IJobContext context)
            {
                throw new ArgumentException("");
            }

            OnRun(context);
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
}
