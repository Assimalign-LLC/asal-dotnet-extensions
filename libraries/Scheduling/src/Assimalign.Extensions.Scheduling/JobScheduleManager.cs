using Assimalign.Extensions.Scheduling.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Assimalign.Extensions.Scheduling;

public sealed class JobScheduleManager : IJobScheduleManager
{
    private readonly ConcurrentBag<JobSceduleListener> listeners;


    public JobScheduleManager()
    {
        this.listeners = new();
        this.ScheduleMonitor = new JobScheduleMonitorDefault();
    }

    public IEnumerable<IJobSchedule> Schedules => throw new NotImplementedException();

    public IJobScheduleMonitor ScheduleMonitor { get; set; }



    public void Attach(IJobSchedule schedule)
    {
        listeners.Add(new JobSceduleListener(schedule, ScheduleMonitor, new JobScheduleOptions()));
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        foreach (var listener in this.listeners)
        {
            tasks.Add(listener.InitializeAsync(cancellationToken));
        }

        return Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return DisposeAsync().AsTask();
    }

    public ValueTask DisposeAsync()
    {


        //Task.WaitAll(scheduleMonitor.Select(x => x))


        return ValueTask.CompletedTask;
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }


    public static IJobScheduleManager Create(Action<IJobScheduleManager> configure)
    {
        throw new NotImplementedException();
    }




    private class JobSceduleListener : IDisposable
    {
        public const string UnscheduledInvocationReasonKey = "UnscheduledInvocationReason";
        public const string OriginalScheduleKey = "OriginalSchedule";

        private readonly CancellationTokenSource cancellationTokenSource;

        // Since Timer uses an integer internally for it's interval,
        // it has a maximum interval of 24.8 days.
        private static readonly TimeSpan _maxTimerInterval = TimeSpan.FromDays(24);

        private System.Timers.Timer timer;
        private IJobSchedule _schedule;
        private IJobScheduleMonitor scheduleMonitor;
        private JobScheduleOptions scheduleOptions;

        private bool isDisposed;
        private TimeSpan _remainingInterval;

        public JobSceduleListener(IJobSchedule schedule, IJobScheduleMonitor scheduleMonitor, JobScheduleOptions scheduleOptions)
        {
            cancellationTokenSource = new CancellationTokenSource();
            _schedule = schedule;
            this.scheduleMonitor = scheduleMonitor;
            this.scheduleOptions = scheduleOptions;
        }

        internal static TimeSpan MaxTimerInterval
        {
            get
            {
                return _maxTimerInterval;
            }
        }
        public IJobSchedule Schedule { get; }

        public JobScheduleStatus ScheduleStatus { get; set; }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (timer != null && timer.Enabled)
            {
                throw new InvalidOperationException("The listener has already been started.");
            }

            // if schedule monitoring is enabled, record (or initialize)
            // the current schedule status
            bool isPastDue = false;

            // we use DateTime.Now rather than DateTime.UtcNow to allow the local machine to set the time zone. In Azure this will be
            // UTC by default, but can be configured to use any time zone if it makes scheduling easier.
            DateTime now = DateTime.Now;

            if (scheduleMonitor != null)
            {
                // check to see if we've missed an occurrence since we last started.
                // If we have, invoke it immediately.
                ScheduleStatus = await scheduleMonitor.GetStatusAsync(_schedule.Id);
               
                TimeSpan pastDueDuration = await scheduleMonitor.CheckPastDueAsync(_schedule, ScheduleStatus, now);
                isPastDue = pastDueDuration != TimeSpan.Zero;
            }

            if (ScheduleStatus == null)
            {
                // no schedule status has been stored yet, so initialize
                ScheduleStatus = new JobScheduleStatus
                {
                    LastRunTime = default(DateTime),
                    NextRunTime = _schedule.GetNextOccurrence(now)
                };
            }

            //if (isPastDue)
            //{
            //    await InvokeJobFunction(now, isPastDue: true, originalSchedule: ScheduleStatus.NextRunTime);
            //}
            //else if (scheduleOptions.RunOnStartup)
            //{
            //    // The job is configured to run immediately on startup
            //    await InvokeJobFunction(now, runOnStartup: true);
            //}

            StartTimer(DateTime.Now);

            while(!isDisposed)
            {

            }
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            bool timerStarted = false;

            try
            {
                e.
                var isPastDue = false;
                DateTime? originalSchedule = ScheduleStatus.NextRunTime;
                DateTime invocationTime = DateTime.Now;
                CancellationToken token = cancellationTokenSource.Token;
                JobScheduleStatus timerInfoStatus = null;
                if (scheduleMonitor != null)
                {
                    timerInfoStatus = ScheduleStatus;
                }


                // Build up trigger details that will be logged if the timer is running at a different time 
                // than originally scheduled.
                IDictionary<string, string> details = new Dictionary<string, string>();
                if (isPastDue)
                {
                    details[UnscheduledInvocationReasonKey] = "IsPastDue";
                }
                else if (scheduleOptions.RunOnStartup)
                {
                    details[UnscheduledInvocationReasonKey] = "RunOnStartup";
                }

                if (originalSchedule.HasValue)
                {
                    details[OriginalScheduleKey] = originalSchedule.Value.ToString("o");
                }

                try
                {
                    _schedule.Run();
                }
                catch
                {
                    // We don't want any function errors to stop the execution
                    // schedule. Invocation errors are already logged.
                }

                if (_remainingInterval != TimeSpan.Zero)
                {
                    // if we're in the middle of a long interval that exceeds
                    // Timer's max interval, continue the remaining interval w/o
                    // invoking the function
                    StartTimer(_remainingInterval);
                    timerStarted = true;
                    return;
                }


                // If the trigger fired before it was officially scheduled (likely under 1 second due to clock skew),
                // adjust the invocation time forward for the purposes of calculating the next occurrence.
                // Without this, it's possible to set the 'Next' value to the same time twice in a row, 
                // which results in duplicate triggers if the site restarts.
                DateTime adjustedInvocationTime = invocationTime;
                if (!isPastDue && !scheduleOptions.RunOnStartup && ScheduleStatus?.NextRunTime > invocationTime)
                {
                    adjustedInvocationTime = ScheduleStatus.NextRunTime;
                }

                // Create the Last value with the adjustedInvocationTime; otherwise, the listener will
                // consider this a schedule change when the host next starts.
                ScheduleStatus = new JobScheduleStatus
                {
                    LastRunTime = adjustedInvocationTime,
                    NextRunTime = _schedule.GetNextOccurrence(adjustedInvocationTime),
                    LastUpdateTime = adjustedInvocationTime
                };

                if (scheduleMonitor != null)
                {
                    scheduleMonitor.UpdateStatusAsync(_schedule.Id, ScheduleStatus).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                // ensure background exceptions don't stop the execution schedule
            }
            finally
            {
                if (!timerStarted)
                {
                    StartTimer(DateTime.Now);
                }
            }
        }

        private void StartTimer(DateTime now)
        {
            var interval = GetNextTimerInterval(ScheduleStatus.NextRunTime, now, scheduleOptions.AdjustForDST);
            // Restart the timer with the next schedule occurrence, but only 
            // if Cancel, Stop, and Dispose have not been called.
            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }
            timer = new System.Timers.Timer
            {
                AutoReset = false
            };
            timer.Elapsed += OnTimer;

            if (interval > MaxTimerInterval)
            {
                // if the interval exceeds the maximum interval supported by Timer,
                // store the remainder and use the max
                _remainingInterval = interval - MaxTimerInterval;
                interval = MaxTimerInterval;
            }
            else
            {
                // clear out any remaining interval
                _remainingInterval = TimeSpan.Zero;
            }

            timer.Interval = interval.TotalMilliseconds;
            timer.Enabled = true;
        }



        /// <summary>
        /// Calculate the next timer interval based on the current (Local) time.
        /// </summary>
        /// <remarks>
        /// We calculate based on the current time because we don't know how long
        /// the previous function invocation took. Example: if you have an hourly timer
        /// invoked at 12:00 and the invocation takes 1 minute, we want to calculate
        /// the interval for the next timer using 12:01 rather than at 12:00. Otherwise, 
        /// you'd start a 1-hour timer at 12:01 when we really want it to be a 59-minute timer.
        /// </remarks>
        /// <param name="next">The next schedule occurrence in Local time.</param>
        /// <param name="now">The current Local time.</param>
        /// <returns>The next timer interval.</returns>
        internal static TimeSpan GetNextTimerInterval(DateTime next, DateTime now, bool adjustForDST)
        {
            TimeSpan nextInterval;

            if (adjustForDST)
            {
                // For calculations, we use DateTimeOffsets and TimeZoneInfo to ensure we honor time zone
                // changes (e.g. Daylight Savings Time)
                var nowOffset = new DateTimeOffset(now, TimeZoneInfo.Local.GetUtcOffset(now));
                var nextOffset = new DateTimeOffset(next, TimeZoneInfo.Local.GetUtcOffset(next));
                nextInterval = nextOffset - nowOffset;
            }
            else
            {
                nextInterval = next - now;
            }

            // If the interval happens to be negative (due to slow storage, for example), adjust the
            // interval back up 1 Tick (Zero is invalid for a timer) for an immediate invocation.
            if (nextInterval <= TimeSpan.Zero)
            {
                nextInterval = TimeSpan.FromTicks(1);
            }

            return nextInterval;
        }

        

        public void Dispose()
        {
            if (!isDisposed)
            {
                // Running callers might still be using the cancellation token.
                // Mark it canceled but don't dispose of the source while the callers are running.
                // Otherwise, callers would receive ObjectDisposedException when calling token.Register.
                // For now, rely on finalization to clean up _cancellationTokenSource's wait handle (if allocated).
                cancellationTokenSource.Cancel();

                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }

                isDisposed = true;
            }
        }
        private void ThrowIfDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(null);
            }
        }
    }
}
