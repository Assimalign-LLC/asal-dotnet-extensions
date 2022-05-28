using System;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

public abstract class JobScheduleMonitor : IJobScheduleMonitor
{
    /// <inheritdoc />
    public abstract Task<JobScheduleStatus> GetStatusAsync(string scheduleId);

    /// <inheritdoc />
    public abstract Task UpdateStatusAsync(string scheduleId, JobScheduleStatus scheduleStatus);

    /// <inheritdoc />
    public virtual async Task<TimeSpan> CheckPastDueAsync(IJobSchedule schedule, JobScheduleStatus lastStatus, DateTime now)
    {
        DateTime recordedNextOccurrence;
        
        if (lastStatus is null)
        {
            // If we've never recorded a status for this timer, write an initial
            // status entry. This ensures that for a new timer, we've captured a
            // status log for the next occurrence even though no occurrence has happened yet
            // (ensuring we don't miss an occurrence)
            DateTime nextOccurrence = schedule.GetNextOccurrence(now);
            lastStatus = new JobScheduleStatus
            {
                LastRunTime = default(DateTime),
                NextRunTime = nextOccurrence,
                LastUpdateTime = now
            };
            await UpdateStatusAsync(schedule.Id, lastStatus);
            recordedNextOccurrence = nextOccurrence;
        }
        else
        {
            DateTime expectedNextOccurrence;

            // Track the time that was used to create 'expectedNextOccurrence'.
            DateTime lastUpdated;

            if (lastStatus.LastRunTime != default(DateTime))
            {
                // If we have a 'Last' value, we know that we used this to calculate 'Next'
                // in a previous invocation. 
                expectedNextOccurrence = schedule.GetNextOccurrence(lastStatus.LastRunTime);
                lastUpdated = lastStatus.LastRunTime;
            }
            else if (lastStatus.LastUpdateTime != default(DateTime))
            {
                // If the trigger has never fired, we won't have 'Last', but we will have
                // 'LastUpdated', which tells us the last time that we used to calculate 'Next'.
                expectedNextOccurrence = schedule.GetNextOccurrence(lastStatus.LastUpdateTime);
                lastUpdated = lastStatus.LastUpdateTime;
            }
            else
            {
                // If we do not have 'LastUpdated' or 'Last', we don't have enough information to 
                // properly calculate 'Next', so we'll calculate it from the current time.
                expectedNextOccurrence = schedule.GetNextOccurrence(now);
                lastUpdated = now;
            }

            // ensure that the schedule hasn't been updated since the last
            // time we checked, and if it has, update the status to use the new schedule
            if (lastStatus.NextRunTime != expectedNextOccurrence)
            {
                // if the schedule has changed and the next occurrence is in the past,
                // recalculate it based on the current time as we don't want it to register
                // immediately as 'past due'.
                if (now > expectedNextOccurrence)
                {
                    expectedNextOccurrence = schedule.GetNextOccurrence(now);
                    lastUpdated = now;
                }

                lastStatus.LastRunTime = default(DateTime);
                lastStatus.NextRunTime = expectedNextOccurrence;
                lastStatus.LastUpdateTime = lastUpdated;
                await UpdateStatusAsync(schedule.Id, lastStatus);
            }
            recordedNextOccurrence = lastStatus.NextRunTime;
        }

        if (now > recordedNextOccurrence)
        {
            // if now is after the last next occurrence we recorded, we know we've missed
            // at least one schedule instance and we are past due
            return now - recordedNextOccurrence;
        }
        else
        {
            // not past due
            return TimeSpan.Zero;
        }
    }
}
