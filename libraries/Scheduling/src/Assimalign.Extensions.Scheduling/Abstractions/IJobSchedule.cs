using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Assimalign.Extensions.Scheduling;

/// <summary>
/// 
/// </summary>
public interface IJobSchedule : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// A unique identifier for the Job Schedule.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    Timer Timer { get; }

    /// <summary>
    /// The initial date in which the schedule was started.
    /// </summary>
    DateTime StartDate { get; }

    /// <summary>
    /// The initial time in which the schedule was started.
    /// </summary>
    TimeSpan StartTime { get; }

    /// <summary>
    /// A timestamp indicating the last runtime.
    /// </summary>
    DateTime LastRunTime { get; }

    /// <summary>
    /// A timestamp indicating the next runtime.
    /// </summary>
    DateTime NextRunTime { get; }

    /// <summary>
    /// A collection of Jobs to be scheduled.
    /// </summary>
    IEnumerable<IJob> Jobs { get; }

    /// <summary>
    /// Get's the current runtime status.
    /// </summary>
    JobScheduleStatus Status { get; }

    /// <summary>
    /// Attaches a job to the given schedule.
    /// </summary>
    /// <param name="job"></param>
    /// <returns></returns>
    IJobSchedule AttachJob(IJob job);

    /// <summary>
    /// 
    /// </summary>
    void OnStart();

    /// <summary>
    /// 
    /// </summary>
    void OnStop();
}