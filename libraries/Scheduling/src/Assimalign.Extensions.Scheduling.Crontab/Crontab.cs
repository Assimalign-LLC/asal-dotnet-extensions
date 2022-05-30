using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling;

/// <summary>
/// 
/// </summary>
public readonly struct CrontabField
{
   // static readonly CompareInfo Comparer = CultureInfo.InvariantCulture.CompareInfo;

    private readonly string expression;
    private readonly CrontabFieldKind kind;
    private readonly int minValue;
    private readonly int maxValue;
    private readonly int[] occurances;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="expression"></param>
    private CrontabField(CrontabFieldKind kind, string expression, int minValue, int maxValue, int[] occurances)
    {
        this.kind = kind;
        this.expression = expression;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.occurances = occurances;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAny => this.Expression == "*";
    public int[] Occurances => this.occurances;
    /// <summary>
    /// 
    /// </summary>
    public CrontabFieldKind Kind => this.kind;
    /// <summary>
    /// 
    /// </summary>
    public string Expression => this.expression;
    /// <summary>
    /// 
    /// </summary>
    public int MaxValue => this.maxValue;
    /// <summary>
    /// 
    /// </summary>
    public int MinValue => this.minValue;
    /// <summary>
    /// 
    /// </summary>
    public int ValueCount => MaxValue - MinValue + 1;


    public static CrontabField Parse(CrontabFieldKind kind, string expression)
    {
        return kind switch
        {
            CrontabFieldKind.Second => ParseSecond(expression),
            CrontabFieldKind.Minute => ParseMinute(expression),
            CrontabFieldKind.Hour => ParseHour(expression),
            CrontabFieldKind.DayOfMonth => ParseDayOfMonth(expression),
            CrontabFieldKind.Month => ParseMonth(expression),
            CrontabFieldKind.DayOfWeek => ParseDayOfWeek(expression)
        };
    }
    private static CrontabField ParseSecond(string expression)
    {
        var current = string.Empty;

        var min = 0;
        var max = 59;

        if (expression == "*")
        {
            var occurances = new int[60];
            for (int i = min; i < max + 1; i++)
            {
                occurances[i] = i;
            }

            return new CrontabField(CrontabFieldKind.Second, expression, min, max, occurances);
        }

        if (expression.Split('/').Length > 1)
        {
            current = expression.Split('/')[0];
        }



        if (rangeIndex > 0)
        {
            var range = expression.Split('-');

            min = int.Parse(expression.Substring(0, rangeIndex - 1));
            max = int.Parse(expression.Substring(rangeIndex + 1, )));

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("");
            }
        }

        return new CrontabField(CrontabFieldKind.Second, expression, min, max, occurance);
    }
    private static CrontabField ParseMinute(string expression)
    {
        var min = 0;
        var max = 59;

        // Let's check to see if any minute is okay
        if (expression == "*")
        {
            var occurrences = new int[60];
            for (int i = min; i < max + 1; i++)
            {
                occurrences[i] = i;
            }
            return new CrontabField(CrontabFieldKind.Minute, expression, min, max, occurrences);
        }
        if (expression.IndexOf('/') > 0)
        {
            var occurrences = new List<int>();
            var step = expression.Split('/');
            var minute = step[0];
            var interval = step[1];

            if (minute == "*")
            {
                // Indicates a list of varied intervals between 0 and 59
                // Example: */2,5,7
                //      Occurrence A: 2, 4, 6, 8,...
                //      Occurrence B: 5, 10, 15,....
                //      Occurrence C: 7, 14, 21, 28,...
                // NOTE: Once the occurrence list has been built, select only distinct int.
                //       The varied interval can sometimes have duplicate values delimiter
                if (interval.IndexOf(',') > 0)
                {
                    var intervals = interval.Split(',');

                    for (int i = 0; i < intervals.Length; i++)
                    {
                        var intervalValue = int.Parse(intervals[i]);

                        for (int a = intervalValue; a <=max; a = a + intervalValue)
                        {
                            occurrences.Add(a);
                        }
                    }
                }

                return new CrontabField(CrontabFieldKind.Minute, expression, min, max, occurrences.Distinct().ToArray());
            }
            if (minute.IndexOf('-') > 0)
            {

            }


        }
        if (expression.IndexOf('-') > 0)
        {

        }
        else
        {
            return new CrontabField(CrontabFieldKind.Minute, expression, min, max, new int[1] { int.Parse(expression) });
        }

        var rangeIndex = expression.IndexOf('-');

        if (rangeIndex > 0)
        {

        }

        throw new NotImplementedException();
    }

   // private static int[] GetOccurances(int min, int max, )
    private static CrontabField ParseHour(string expression)
    {
        var min = 0;
        var max = 23;

        if (expression == "*")
        {
            var occurances = new int[24];
            for (int i = min; i < max + 1; i++)
            {
                occurances[i] = i;
            }
            return new CrontabField(CrontabFieldKind.Hour, expression, min, max, occurances);
        }

        throw new NotImplementedException();
    }
    private static CrontabField ParseDayOfMonth(string expression)
    {
        var min = 1;
        var max = 31;

        if (expression == "*")
        {
            var occurances = new int[31];
            for (int i = min; i < max + 1; i++)
            {
                occurances[i] = i;
            }
            return new CrontabField(CrontabFieldKind.DayOfMonth, expression, min, max, occurances);
        }

        throw new NotImplementedException();
    }
    private static CrontabField ParseMonth(string expression)
    {
        var min = 1;
        var max = 12;

        if (expression == "*")
        {
            var occurances = new int[12];
            for (int i = min; i < max + 1; i++)
            {
                occurances[i] = i;
            }
            return new CrontabField(CrontabFieldKind.Month, expression, min, max, occurances);
        }


        throw new NotImplementedException();
    }
    private static CrontabField ParseDayOfWeek(string expression)
    {
        var min = 0;
        var max = 6;

        if (expression == "*")
        {
            var occurances = new int[7];
            for (int i = min; i < max + 1; i++)
            {
                occurances[i] = i;
            }
            return new CrontabField(CrontabFieldKind.DayOfWeek, expression, min, max, occurances);
        }

       

        throw new NotImplementedException();
    }

    public static int[] GetOccurances(string expression, int min, int max)
    {
        if (expression == "*")
        {
            var seed = min;
            var occurrences = new int[max - min + 1];
            for (int i = 0; i < occurrences.Length; i++)
            {
                occurrences[i] = seed;
                seed++;
            }
            return occurrences;
        }
        if (expression.Contains('/'))
        {
            var steps = expression.Split('/');
            var boundariesStep = steps[0];
            var intervalsStep = steps[1];

            // Check for invalid setp format
            if (steps.Length > 2)
            {
                throw new FormatException($"The following expression '{expression}' has more than one step delimiter -> '/'.");
            }
            if (boundariesStep.Equals("*"))
            {
                var occurrences = new List<int>();
                // Indicates a list of varied intervals between 0 and 59
                // Example: */2,5,7
                //      Occurrence A: 2, 4, 6, 8,...
                //      Occurrence B: 5, 10, 15,....
                //      Occurrence C: 7, 14, 21, 28,...
                // NOTE: Once the occurrence list has been built, select only distinct int.
                //       The varied interval can sometimes have duplicate values
                if (intervalsStep.Contains(','))
                {
                    var intervals = intervalsStep.Split(',');

                    for (int i = 0; i < intervals.Length; i++)
                    {
                        occurrences.AddRange(GetOccurances($"*/{intervals[i]}", min, max));
                    }
                }
                else
                {
                    var interval = int.Parse(intervalsStep);
                    if (interval > max)
                    {
                        throw new ArgumentException($"The step value '{interval}' in expression '{expression}' cannot be greater than '{max}'.");
                    }
                    for (int i = min; i <= max; i = i + interval)
                    {
                        occurrences.Add(i);
                    }
                }

                occurrences.Sort();
                return occurrences.Distinct().ToArray();
            }
            // Check for a list of boundaries
            if (boundariesStep.Contains(','))
            {
                var occurrences = new List<int>();
                var boundariesList = boundariesStep.Split(',');

                for (int i = 0; i < boundariesList.Length; i++)
                {
                    var lower = min;
                    var upper = max;

                    // Is the current boundary a range or single value
                    if (boundariesList[i].Contains('-'))
                    {
                        lower = int.Parse(boundariesList[i].Split('-')[0]);
                        upper = int.Parse(boundariesList[i].Split('-')[1]);
                    }
                    else
                    {
                        lower = int.Parse(boundariesList[i]);
                    }
                    // Check if the parse boundaries are greater of less than the defualt boundaries
                    if (lower < min || upper > max)
                    {
                        throw new ArgumentOutOfRangeException("");
                    }
                    // Now check if the intervals step is also a list
                    if (intervalsStep.Contains(','))
                    {
                        var intervals = intervalsStep.Split(',');

                        for (int c = 0; c < intervals.Length; c++)
                        {
                            occurrences.AddRange(GetOccurances($"*/{intervals[c]}", lower, upper));
                        }
                    }
                    else
                    {
                        occurrences.AddRange(GetOccurances($"*/{intervalsStep}", lower, upper));
                    }
                }

                occurrences.Sort();
                return occurrences.Distinct().ToArray();
            }
            // Check if boundaries is a range
            if (boundariesStep.Contains('-'))
            {
                var lower = int.Parse(boundariesStep.Split('-')[0]);
                var upper = int.Parse(boundariesStep.Split('-')[1]);
                var occurrences = new List<int>();

                // Check if the parse boundaries are greater of less than the defualt boundaries
                if (lower < min || upper > max)
                {
                    throw new ArgumentOutOfRangeException("");
                }
                if (intervalsStep.Contains(','))
                {
                    var intervals = intervalsStep.Split(',');
                    for (int i = 0; i < intervals.Length; i++)
                    {
                        occurrences.AddRange(GetOccurances($"{lower}-{upper}/{intervals[i]}", min, max));
                    }
                }
                else
                {
                    var interval = int.Parse(intervalsStep);

                    for (int i = lower; i < upper; i = i + interval)
                    {
                        occurrences.Add(i);
                    }
                }

                occurrences.Sort();
                return occurrences.Distinct().ToArray();
            }
        }
        if (expression.Contains(','))
        {
            var boundaies = expression.Split(',');
            var occurrences = new List<int>();

            for (int i = 0; i < boundaies.Length; i++)
            {
                if (boundaies[i].Contains('-'))
                {
                    var lower = int.Parse(boundaies[i].Split('-')[0]);
                    var upper = int.Parse(boundaies[i].Split('-')[1]);

                    occurrences.AddRange(GetOccurances($"{lower}-{upper}", min, max));
                }
                else
                {
                    occurrences.AddRange(GetOccurances(boundaies[i], min, max));
                }
            }

            occurrences.Sort();
            return occurrences.ToArray();
        }
        if (expression.Contains('-'))
        {
            var lower = int.Parse(expression.Split('-')[0]);
            var upper = int.Parse(expression.Split('-')[1]);
            var occurrences = new List<int>();

            //
            for (int i = lower; i < upper + 1; i++)
            {
                occurrences.Add(i);
            }

            occurrences.Sort();
            return occurrences.ToArray();
        }
        else
        {
            return new int[1] { int.Parse(expression) };
        }
    }

}


/// <summary>
/// 
/// </summary>
/// <![CDATA[
/// Crontab expression format:
///
/// * * * * *
/// - - - - -
/// | | | | |
/// | | | | +----- day of week (0 - 6) (Sunday=0)
/// | | | +------- month (1 - 12)
/// | | +--------- day of month (1 - 31)
/// | +----------- hour (0 - 23)
/// +------------- min (0 - 59)
///
/// Star (*) in the value field above means all legal values as in
/// braces for that column. The value column can have a * or a list
/// of elements separated by commas. An element is either a number in
/// the ranges shown above or two numbers in the range separated by a
/// hyphen (meaning an inclusive range).
///
/// Source: http://www.adminschoice.com/docs/crontab.htm
///
///
/// Six-part expression format:
///
/// * * * * * *
/// - - - - - -
/// | | | | | |
/// | | | | | +--- day of week (0 - 6) (Sunday=0)
/// | | | | +----- month (1 - 12)
/// | | | +------- day of month (1 - 31)
/// | | +--------- hour (0 - 23)
/// | +----------- min (0 - 59)
/// +------------- sec (0 - 59)
///
/// The six-part expression behaves similarly to the traditional
/// crontab format except that it can denotate more precise schedules
/// that use a seconds component.
/// ]]>
public readonly struct Crontab : IFormattable, IEquatable<Crontab>, IEnumerable<DateTime>
{
    public const char RangValue = '-';
    public const char StepValue = '/';
    public const char Any = '*';
    public const char ListSeparator = ',';


    private readonly CrontabField second;      // index - 0 or 1
    private readonly CrontabField minute;      // index - 1 or 2
    private readonly CrontabField hour;
    private readonly CrontabField dayOfMonth;
    private readonly CrontabField month;
    private readonly CrontabField dayOfWeek;

    private Crontab(CrontabField second, CrontabField minute, CrontabField hour, CrontabField dayOfMonth, CrontabField month, CrontabField dayOfWeek)
    {
        this.second = second;
        this.minute = minute;
        this.hour = hour;
        this.dayOfMonth = dayOfMonth;
        this.month = month;
        this.dayOfWeek = dayOfWeek;
    }

    public CrontabField Second => this.second;
    public CrontabField Minute => this.minute;
    public CrontabField Hour => this.hour;
    public CrontabField DayOfMonth => this.dayOfMonth;
    public CrontabField Month => this.month;
    public CrontabField DayOfWeek => this.dayOfWeek;

    public DateTime GetNextOccurance()
    {
        var now = DateTime.Now;

        while (true)
        {

            foreach (var month in month.Occurances)
            {
                if (Month.IsAny)
                {

                }
                foreach (var dayOfMonth in DayOfMonth.Occurances)
                {
                    foreach (var dayOfWeek in DayOfWeek.Occurances)
                    {

                        foreach (var hour in Hour.Occurances)
                        {
                            foreach (var minute in Minute.Occurances)
                            {
                                if (Second.IsAny)
                                {

                                }
                                foreach (var second in Second.Occurances)
                                {

                                }
                            }
                        }
                    }
                }
            }

            now.AddSeconds(1);
        }


        //while (true)
        //{
        //    restart:
        //    foreach (var month in Month.Occurances)
        //    {
        //        if (month <= now.Month)
        //        {
        //            continue;
        //        }
        //        // Let's increment the month up if need be
        //        while (now.Month < month)
        //        {
        //            now.AddMonths(1);
        //        }



        //        var t = DayOfMonth.Occurances.Length * DayOfWeek.Occurances.Length;



        //        foreach (var dayOfMonth in DayOfMonth.Occurances)
        //        {

        //            if (dayOfMonth >= now.Day)
        //            {
        //                // Let's increment the month up
        //                while (now.Day < dayOfMonth)
        //                {
        //                    now.AddDays(1);
        //                }
        //                foreach (var dayOfWeek in )
        //            }
        //        }
        //    }
        //}

        return now;
    }

    // Sorts values by array length an returns them
    // month - dayofMonth - dayOfWeek - hour - minute - second
    private IEnumerable<Tuple<int, int, int, int, int, int>> GetCombinationTree()
    {
        var cobinations = new List<Tuple<int, int, int, int, int, int>>();

        foreach (var month in month.Occurances)
        {
            foreach (var dayOfMonth in DayOfMonth.Occurances)
            {
                foreach (var dayOfWeek in DayOfWeek.Occurances)
                {
                    foreach (var hour in Hour.Occurances)
                    {
                        foreach (var minute in Minute.Occurances)
                        {
                            foreach (var second in Second.Occurances)
                            {
                                yield return new Tuple<int, int, int, int, int, int>(month, dayOfMonth, dayOfWeek, hour, minute, second);
                            }
                        }
                    }
                }
            }
        }

        //  return cobinations;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Equals(Crontab other)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object instance) => instance is Crontab crontab ? Equals(crontab) : false;
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return base.ToString();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string ToString(string format, IFormatProvider formatProvider)
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(Crontab left, Crontab right) => left.Equals(right);
    public static bool operator !=(Crontab left, Crontab right) => !left.Equals(right);
    public static implicit operator Crontab(string expression) => Crontab.Parse(expression);


    public static Crontab Parse(string expression)
    {
        var segments = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Let's ensure that the expression segments has the proper length 
        if (segments.Length != 5 && segments.Length != 6)
        {
            throw new ArgumentException("The expression is not in the proper format.");
        }

        var second      = default(CrontabField);      // index - 0 or 1
        var minute      = default(CrontabField);      // index - 1 or 2
        var hour        = default(CrontabField);
        var dayOfMonth  = default(CrontabField);
        var month       = default(CrontabField);
        var dayOfWeek   = default(CrontabField);

        var offset = segments.Length == 6 ? 0 : 1;

        for (int i = 0; i < segments.Length; i++)
        {
            var kind = (CrontabFieldKind)i + offset;

            if ((i + offset) == 0)
            {
                second = CrontabField.Parse(kind, segments[i]);
                continue;
            }
            if ((i + offset) == 1)
            {
                minute = CrontabField.Parse(kind, segments[i]);
                continue;
            }
            if ((i + offset) == 2)
            {
                hour = CrontabField.Parse(kind, segments[i]);
                continue;
            }
            if ((i + offset) == 3)
            {
                dayOfMonth = CrontabField.Parse(kind, segments[i]);
                continue;
            }
            if ((i + offset) == 4)
            {
                month = CrontabField.Parse(kind, segments[i]);
                continue;
            }
            if ((i + offset) == 5)
            {
                dayOfWeek = CrontabField.Parse(kind, segments[i]);
                continue;
            }
        }

        return new Crontab(second, minute, hour, dayOfMonth, month, dayOfWeek);
    }

    private static CrontabField ParseMinute(string expression)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<DateTime> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
