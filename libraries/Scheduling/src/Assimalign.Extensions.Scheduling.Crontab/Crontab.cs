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
public readonly struct Crontab: IEquatable<Crontab>, IEnumerable<DateTime> // IFormattable, IEquatable<Crontab>, IEnumerable<DateTime>
{
	public const char RangValue = '-';
	public const char StepValue = '/';
	public const char Any = '*';
	public const char ListSeparator = ',';

	private Crontab(CrontabField minute, CrontabField hour, CrontabField dayOfMonth, CrontabField month, CrontabField dayOfWeek)
	{
		this.Minute = minute;
		this.Hour = hour;
		this.DayOfMonth = dayOfMonth;
		this.Month = month;
		this.DayOfWeek = dayOfWeek;
	}

	public CrontabField Minute { get; }
	public CrontabField Hour { get; }
	public CrontabField DayOfMonth { get; }
	public CrontabField Month { get; }
	public CrontabField DayOfWeek { get; }

	/// <summary>
	/// Get's the amount of time until the next occurrence from the current local time.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public TimeSpan GetTimeSpan()
    {
		return GetNextDateTime().Subtract(DateTime.Now);
    }

	/// <summary>
	/// Get's the next occurrence in DateTime from the current local time.
	/// </summary>
	/// <returns></returns>
	public DateTime GetDateTime()
	{
		return GetDateTime(DateTime.Now);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="start"></param>
	/// <returns></returns>
	public DateTime GetDateTime(DateTime start)
    {
		var next = new DateTime(start.Year, 1, 1, 0, 0, 0);

	restart:
		for (int a = 0; a < 12; a++)
		{
			if (!Month.Occurances.Contains(a + 1))
			{
				next = next.AddMonths(1);
				continue;
			}
			for (int b = 0; b < DateTime.DaysInMonth(next.Year, a + 1); b++)
			{
				if (!DayOfMonth.Occurances.Contains(b + 1) || !DayOfWeek.Occurances.Contains((int)next.DayOfWeek))
				{
					next = next.AddDays(1);
					continue;
				}
				for (int c = 0; c < 24; c++)
				{
					if (!Hour.Occurances.Contains(c))
					{
						next = next.AddHours(1);
						continue;
					}
					for (int d = 0; d < 60; d++)
					{
						if (Minute.Occurances.Contains(d) && next > start)
						{
							return next;
						}
						else
						{
							next = next.AddMinutes(1);
						}
					}
				}
			}
		}

		goto restart;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Equals(Crontab other)
    {
        return 
			this.Minute.Expression == other.Minute.Expression &&
			this.Hour.Expression == other.Hour.Expression &&
			this.DayOfMonth.Expression == other.DayOfMonth.Expression &&
			this.Month.Expression == other.Month.Expression &&
			this.DayOfWeek.Expression == other.DayOfWeek.Expression;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object instance) => instance is Crontab crontab ? Equals(crontab) : false;

    public static bool operator ==(Crontab left, Crontab right) => left.Equals(right);
	public static bool operator !=(Crontab left, Crontab right) => !left.Equals(right);

	public static implicit operator Crontab(string expression) => Crontab.Parse(expression);


	public static Crontab Parse(string expression)
	{
		var segments = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		// Let's ensure that the expression segments has the proper length 
		if (segments.Length != 5)
		{
			throw new ArgumentException("The expression is not in the proper format.");
		}

		var minute = default(CrontabField);      // index - 1 or 2
		var hour = default(CrontabField);
		var dayOfMonth = default(CrontabField);
		var month = default(CrontabField);
		var dayOfWeek = default(CrontabField);

		for (int i = 0; i < segments.Length; i++)
		{
			if (i == 0)
			{
				minute = CrontabField.ParseMinute(segments[i]);
				continue;
			}
			if (i == 1)
			{
				hour = CrontabField.ParseHour(segments[i]);
				continue;
			}
			if (i == 2)
			{
				dayOfMonth = CrontabField.ParseDayOfMonth(segments[i]);
				continue;
			}
			if (i == 3)
			{
				month = CrontabField.ParseMonth(segments[i]);
				continue;
			}
			if (i == 4)
			{
				dayOfWeek = CrontabField.ParseDayOfWeek(segments[i]);
				continue;
			}
		}

		return new Crontab(minute, hour, dayOfMonth, month, dayOfWeek);
	}

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    public IEnumerator<DateTime> GetEnumerator()
    {
		return new CrontabEnumerator(this);
    }

    private class CrontabEnumerator : IEnumerator<DateTime>
    {
		private readonly Crontab crontab;
		private DateTime? index;

        public CrontabEnumerator(Crontab crontab)
        {
			this.crontab = crontab;
        }

        public DateTime Current
        {
			get
            {
				if (!index.HasValue)
                {
					index = crontab.GetDateTime();
					return index.GetValueOrDefault();
                }
				else
                {
					index = crontab.GetDateTime(index.Value);
					return index.GetValueOrDefault();
                }
            }
        }

		object IEnumerator.Current => this.Current;

        public void Dispose()
        {
            
        }

		public bool MoveNext() => true;

        public void Reset()
        {
            index 
        }
    }
}
