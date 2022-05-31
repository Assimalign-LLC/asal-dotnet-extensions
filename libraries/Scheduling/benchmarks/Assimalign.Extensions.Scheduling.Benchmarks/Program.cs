using Assimalign.Extensions.Scheduling;
using System.Diagnostics;
//using System.Threading;


Crontab crontab = "0 1 * * *";
long i = 0;
var timer = new Stopwatch();

timer.Start();

crontab.GetDateTime();

timer.Stop();
Console.WriteLine(timer.ElapsedTicks);

timer.Restart();

crontab.GetDateTime();

timer.Stop();
Console.WriteLine(timer.ElapsedTicks);

//foreach (var item in crontab)
//{
//    if ((i % 10000) == 1000)
//    {
//        Console.WriteLine(item);
//    }
//    i++;
//}



//var manager = new JobScheduleManager();

//manager.Attach(new ConstantJobSchedule(TimeSpan.FromSeconds(3)));
////manager.Attach(new DailySchedule("12:44:00"));

//manager.StartAsync(CancellationToken.None).GetAwaiter().GetResult();



//public class DailySchedule : JobSchedule
//{
//    private readonly List<TimeSpan> schedule = new List<TimeSpan>();

//    /// <summary>
//    /// Constructs a new instance.
//    /// </summary>
//    public DailySchedule()
//    {
//    }

//    /// <summary>
//    /// Constructs an instance based on the specified collection of
//    /// <see cref="TimeSpan"/> strings.
//    /// </summary>
//    /// <param name="times">The daily schedule times.</param>
//    public DailySchedule(params string[] times)
//    {
//        schedule = times.Select(p => TimeSpan.Parse(p)).OrderBy(p => p).ToList();
//    }

//    /// <summary>
//    /// Constructs an instance based on the specified collection of
//    /// <see cref="TimeSpan"/> instances.
//    /// </summary>
//    /// <param name="times">The daily schedule times.</param>
//    public DailySchedule(params TimeSpan[] times)
//    {
//        schedule = times.OrderBy(p => p).ToList();
//    }

//    /// <inheritdoc/>
//    public override DateTime GetNextOccurrence(DateTime now)
//    {
//        if (schedule.Count == 0)
//        {
//            throw new InvalidOperationException("The schedule is empty.");
//        }

//        // find the next occurrence in the schedule where the time
//        // is strictly greater than now
//        int idx = schedule.FindIndex(p => now.TimeOfDay < p);
//        if (idx == -1)
//        {
//            // no more occurrences for today, so start back at the beginning of the
//            // the schedule tomorrow
//            TimeSpan nextTime = schedule[0];
//            DateTime nextOccurrence = new DateTime(now.Year, now.Month, now.Day, nextTime.Hours, nextTime.Minutes, nextTime.Seconds, now.Kind);
//            return nextOccurrence.AddDays(1);
//        }
//        else
//        {
//            TimeSpan nextTime = schedule[idx];
//            return new DateTime(now.Year, now.Month, now.Day, nextTime.Hours, nextTime.Minutes, nextTime.Seconds, now.Kind);
//        }
//    }

//    public override IEnumerable<DateTime> GetNextOccurrences(int count, DateTime? now = null)
//    {
//        throw new NotImplementedException();
//    }

//    public override void Run()
//    {
//        Console.WriteLine("Running");
//    }

//    /// <inheritdoc/>
//    public override string ToString()
//    {
//        return string.Format("Daily: {0} occurrences", schedule.Count);
//    }
//}


//public class ConstantJobSchedule : JobSchedule
//{
//    private readonly TimeSpan _interval;
//    private TimeSpan? _intervalOverride;

//    public ConstantJobSchedule(TimeSpan interval)
//    {
//        _interval = interval;
//    }

//    public override DateTime GetNextOccurrence(DateTime now)
//    {
//        TimeSpan nextInterval = _interval;
//        if (_intervalOverride != null)
//        {
//            nextInterval = _intervalOverride.Value;
//            _intervalOverride = null;
//        }

//        return now + nextInterval;
//    }

//    public override IEnumerable<DateTime> GetNextOccurrences(int count, DateTime? now = null)
//    {
//        throw new NotImplementedException();
//    }

//    public override void Run()
//    {
//        Console.WriteLine("Running");
//    }
//}
