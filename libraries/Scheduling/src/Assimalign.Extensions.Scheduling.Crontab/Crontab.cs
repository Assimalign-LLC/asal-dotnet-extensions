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
public readonly struct Crontab //: IEnumerable<DateTime> // IFormattable, IEquatable<Crontab>, IEnumerable<DateTime>
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

	public DateTime GetNextOccurance()
	{
		var now = DateTime.Now;



		//		while (true)
		//		{
		//
		//			foreach (var month in month.Occurances)
		//			{
		//				if (Month.IsAny)
		//				{
		//
		//				}
		//				foreach (var dayOfMonth in DayOfMonth.Occurances)
		//				{
		//					foreach (var dayOfWeek in DayOfWeek.Occurances)
		//					{
		//
		//						foreach (var hour in Hour.Occurances)
		//						{
		//							foreach (var minute in Minute.Occurances)
		//							{
		//								if (Second.IsAny)
		//								{
		//
		//								}
		//								foreach (var second in Second.Occurances)
		//								{
		//
		//								}
		//							}
		//						}
		//					}
		//				}
		//			}
		//
		//			now.AddSeconds(1);
		//		}


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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	//public bool Equals(Crontab other)
	//{
	//	throw new NotImplementedException();
	//}
	///// <summary>
	///// 
	///// </summary>
	///// <param name="instance"></param>
	///// <returns></returns>
	//public override bool Equals([NotNullWhen(true)] object instance) => instance is Crontab crontab ? Equals(crontab) : false;
	///// <summary>
	///// 
	///// </summary>
	///// <returns></returns>
	//public override string ToString()
	//{
	//	return base.ToString();
	//}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="format"></param>
	/// <param name="formatProvider"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	//public string ToString(string format, IFormatProvider formatProvider)
	//{
	//	throw new NotImplementedException();
	//}

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

	//	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
	//	public IEnumerator<DateTime> GetEnumerator()
	//	{
	//		throw new NotImplementedException();
	//	}
	//
	//	private class CrontabEnumerator : IEnumerator<DateTime>
	//	{
	//		public CrontabEnumerator(Crontab crontab)
	//		{
	//				
	//		}
	//
	//		public DateTime Current => throw new NotImplementedException();
	//
	//		object IEnumerator.Current => throw new NotImplementedException();
	//
	//		public void Dispose()
	//		{
	//			throw new NotImplementedException();
	//		}
	//
	//		public bool MoveNext()
	//		{
	//			throw new NotImplementedException();
	//		}
	//
	//		public void Reset()
	//		{
	//			throw new NotImplementedException();
	//		}
	//	}
}