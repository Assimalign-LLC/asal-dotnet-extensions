using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public interface IJobScheduleMonitor : IAsyncDisposable, IDisposable
{
    IEnumerable<IJobSchedule> Schedules { get; }

    Task MonitorAsync(CancellationToken cancellationToken);

    ///// <summary>
    ///// Gets the last recorded schedule status for the specified timer.
    ///// If the timer has not ran yet, null will be returned.
    ///// </summary>
    ///// <param name="scheduleId">The unique identifier of the timer to check.</param>
    ///// <returns>The schedule status.</returns>
    //Task<JobScheduleStatus> GetStatusAsync(string scheduleId);

    ///// <summary>
    ///// Updates the schedule status for the specified timer.
    ///// </summary>
    ///// <param name="scheduleId">The unique identifier of the of the schedule.</param>
    ///// <param name="scheduleStatus">The new schedule status.</param>
    //Task UpdateStatusAsync(string scheduleId, JobScheduleStatus scheduleStatus);

    ///// <summary>
    ///// Checks whether the schedule is currently past due.
    ///// </summary>
    ///// <remarks>
    ///// On startup, all schedules are checked to see if they are past due. Any
    ///// timers that are past due will be executed immediately by default. Subclasses can
    ///// change this behavior by inspecting the current time and schedule to determine
    ///// whether it should be considered past due.
    ///// </remarks>
    ///// <param name="schedule">The name of the timer to check.</param>
    ///// <param name="now">The time to check.</param>
    ///// <param name="schedule">The <see cref="TimerSchedule"/>.</param>
    ///// <param name="lastStatus">The last recorded status, or null if the status has never been recorded.</param>
    ///// <returns>A non-zero <see cref="TimeSpan"/> if the schedule is past due, otherwise <see cref="TimeSpan.Zero"/>.</returns>
    //Task<TimeSpan> CheckPastDueAsync(IJobSchedule schedule, JobScheduleStatus lastStatus, DateTime now);
}


public sealed class JobScheduleMonitorDefault : IJobScheduleMonitor
{
    private readonly IEnumerable<IJobSchedule> schedules;
    private readonly JobScheduleOptions scheduleOptions;

    private CancellationTokenSource cancellationTokenSource;

    public JobScheduleMonitorDefault(IEnumerable<IJobSchedule> schedules, JobScheduleOptions scheduleOptions)
    {
        this.schedules = schedules;
        this.scheduleOptions = scheduleOptions;
    }

    public IEnumerable<IJobSchedule> Schedules => this.schedules;

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }
    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.Run(() =>
        {

        }));
    }


    public Task MonitorAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken == default)
        {
            cancellationToken = CancellationToken.None;
        }

        // Let's create a Token source to be used to 
        cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        return Task.Run(async () =>
        {
            while (Timer.ActiveCount > 0)
            {
                
            }
        });
    }
}