using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace Assimalign.Extensions.Scheduling;

public class CrontabFied : ICrontabField
{
    readonly BitArray _bits;
    /* read-only */
    int _minValueSet;
    /* read-only */
    int _maxValueSet;
    readonly CrontabFieldImpl _impl;

    /// <summary>
    /// Parses a crontab field expression given its kind.
    /// </summary>
    public static CrontabFied Parse(CrontabFieldKind kind, string expression) => TryParse(kind, expression, v => v, e => throw e());
    public static CrontabFied? TryParse(CrontabFieldKind kind, string expression) => TryParse(kind, expression, v => v, _ => (CrontabFied?)null);
    public static T TryParse<T>(CrontabFieldKind kind, string expression, Func<CrontabFied, T> valueSelector, Func<ExceptionProvider, T> errorSelector)
    {
        var field = new CrontabFied(CrontabFieldImpl.FromKind(kind));
        var error = field._impl.TryParse(expression, field.Accumulate, (ExceptionProvider?)null, e => e);
        return error == null ? valueSelector(field) : errorSelector(error);
    }

    /// <summary>
    /// Parses a crontab field expression representing seconds.
    /// </summary>
    public static CrontabFied Seconds(string expression) => Parse(CrontabFieldKind.Second, expression);

    /// <summary>
    /// Parses a crontab field expression representing minutes.
    /// </summary>
    public static CrontabFied Minutes(string expression) => Parse(CrontabFieldKind.Minute, expression);

    /// <summary>
    /// Parses a crontab field expression representing hours.
    /// </summary>
    public static CrontabFied Hours(string expression) => Parse(CrontabFieldKind.Hour, expression);

    /// <summary>
    /// Parses a crontab field expression representing days in any given month.
    /// </summary>
    public static CrontabFied Days(string expression) => Parse(CrontabFieldKind.DayOfMonth, expression);

    /// <summary>
    /// Parses a crontab field expression representing months.
    /// </summary>
    public static CrontabFied Months(string expression) => Parse(CrontabFieldKind.Month, expression);

    /// <summary>
    /// Parses a crontab field expression representing days of a week.
    /// </summary>
    public static CrontabFied DaysOfWeek(string expression) => Parse(CrontabFieldKind.DayOfWeek, expression);

    CrontabFied(CrontabFieldImpl impl)
    {
        _impl = impl ?? throw new ArgumentNullException(nameof(impl));
        _bits = new BitArray(impl.ValueCount);
        _minValueSet = int.MaxValue;
        _maxValueSet = -1;
    }

    /// <summary>
    /// Gets the first value of the field or -1.
    /// </summary>
    public int GetFirst() => _minValueSet < int.MaxValue ? _minValueSet : -1;

    /// <summary>
    /// Gets the next value of the field that occurs after the given
    /// start value or -1 if there is no next value available.
    /// </summary>
    public int Next(int start)
    {
        if (start < _minValueSet)
        {
            return _minValueSet;
        }

        var startIndex = ValueToIndex(start);
        var lastIndex = ValueToIndex(_maxValueSet);

        for (var i = startIndex; i <= lastIndex; i++)
        {
            if (_bits[i])
                return IndexToValue(i);
        }

        return -1;
    }

    int IndexToValue(int index) => index + _impl.MinValue;
    int ValueToIndex(int value) => value - _impl.MinValue;

    /// <summary>
    /// Determines if the given value occurs in the field.
    /// </summary>
    public bool Contains(int value) => _bits[ValueToIndex(value)];

    /// <summary>
    /// Accumulates the given range (start to end) and interval of values
    /// into the current set of the field.
    /// </summary>
    /// <remarks>
    /// To set the entire range of values representable by the field,
    /// set <param name="start" /> and <param name="end" /> to -1 and
    /// <param name="interval" /> to 1.
    /// </remarks>
    T Accumulate<T>(int start, int end, int interval, T success, Func<ExceptionProvider, T> errorSelector)
    {
        var minValue = _impl.MinValue;
        var maxValue = _impl.MaxValue;

        if (start == end)
        {
            if (start < 0)
            {
                //
                // We're setting the entire range of values.
                //

                if (interval <= 1)
                {
                    _minValueSet = minValue;
                    _maxValueSet = maxValue;
                    _bits.SetAll(true);
                    return success;
                }

                start = minValue;
                end = maxValue;
            }
            else
            {
                //
                // We're only setting a single value - check that it is in range.
                //

                if (start < minValue)
                    return OnValueBelowMinError(start, errorSelector);

                if (start > maxValue)
                    return OnValueAboveMaxError(start, errorSelector);
            }
        }
        else
        {
            //
            // For ranges, if the start is bigger than the end value then
            // swap them over.
            //

            if (start > end)
            {
                end ^= start;
                start ^= end;
                end ^= start;
            }

            if (start < 0)
                start = minValue;
            else if (start < minValue)
                return OnValueBelowMinError(start, errorSelector);

            if (end < 0)
                end = maxValue;
            else if (end > maxValue)
                return OnValueAboveMaxError(end, errorSelector);
        }

        if (interval < 1)
            interval = 1;

        int i;

        //
        // Populate the _bits table by setting all the bits corresponding to
        // the valid field values.
        //

        for (i = start - minValue; i <= (end - minValue); i += interval)
            _bits[i] = true;

        //
        // Make sure we remember the minimum value set so far Keep track of
        // the highest and lowest values that have been added to this field
        // so far.
        //

        if (_minValueSet > start)
            _minValueSet = start;

        i += (minValue - interval);

        if (_maxValueSet < i)
            _maxValueSet = i;

        return success;
    }

    T OnValueAboveMaxError<T>(int value, Func<ExceptionProvider, T> errorSelector) =>
        errorSelector(
            () => new CrontabException(
                $"{value} is higher than the maximum allowable value for the [{_impl.Kind}] field. " +
                $"Value must be between {_impl.MinValue} and {_impl.MaxValue} (all inclusive)."));

    T OnValueBelowMinError<T>(int value, Func<ExceptionProvider, T> errorSelector) =>
        errorSelector(
            () => new CrontabException(
                $"{value} is lower than the minimum allowable value for the [{_impl.Kind}] field. " +
                $"Value must be between {_impl.MinValue} and {_impl.MaxValue} (all inclusive)."));

    public override string ToString() => ToString(null);

    public string ToString(string? format)
    {
        var writer = new StringWriter(CultureInfo.InvariantCulture);

        switch (format)
        {
            case "G":
            case null:
                Format(writer, true);
                break;
            case "N":
                Format(writer);
                break;
            default:
                throw new FormatException();
        }

        return writer.ToString();
    }

    public void Format(TextWriter writer) => Format(writer, false);

    public void Format(TextWriter writer, bool noNames) =>
        _impl.Format(this, writer, noNames);
}
