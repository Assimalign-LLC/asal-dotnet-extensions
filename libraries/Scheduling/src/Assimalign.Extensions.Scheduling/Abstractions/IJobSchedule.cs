using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

/// <summary>
/// 
/// </summary>
public interface IJobSchedule
{
    /// <summary>
    /// A unique identifier for the Job Schedule.
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// 
    /// </summary>
    IEnumerable<IJob> Jobs { get; }

    /// <summary>
    /// 
    /// </summary>
    JobScheduleStatus ScheduleStatus { get; }

    /// <summary>
    /// Gets the next occurrence of the schedule based on the specified
    /// base time.
    /// </summary>
    /// <param name="now">The time to compute the next schedule occurrence from.</param>
    /// <returns>The next schedule occurrence.</returns>
    DateTime GetNextOccurrence(DateTime now);

    /// <summary>
    /// Returns a collection of the next 'count' occurrences of the schedule,
    /// starting from now.
    /// </summary>
    /// <param name="count">The number of occurrences to return.</param>
    /// <returns>A collection of the next occurrences.</returns>
    /// <param name="now">The optional <see cref="DateTime"/> to start from.</param>
    IEnumerable<DateTime> GetNextOccurrences(int count, DateTime? now = null);

    /// <summary>
    /// Runs all the Jobs for this schedule.
    /// </summary>
    void Run();

    /// <summary>
    /// 
    /// </summary>
    void OnComplete();
}
