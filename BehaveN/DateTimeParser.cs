using System;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Utility class that parses DateTime strings.
    /// </summary>
    public static class DateTimeParser
    {
        private static readonly Regex _inXUnits = new Regex(@"in\s+(\d+)\s+(.+?)s?\b", RegexOptions.IgnoreCase);
        private static readonly Regex _XUnitsAgo = new Regex(@"(\d+)\s+(.+?)s?\s+ago", RegexOptions.IgnoreCase);
        private static readonly Regex _XUnitsFromNow = new Regex(@"(\d+)\s+(.+?)s?\s+from\s+now", RegexOptions.IgnoreCase);
        private static readonly Regex _XDaysFromToday = new Regex(@"(\d+)\s+days?\s+from\s+today", RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The parsed value or <c>DateTime.MinValue</c> if it fails to parse.</returns>
        public static DateTime ParseDateTime(string value)
        {
            return ParseDateTime(value, DateTime.MinValue);
        }

        /// <summary>
        /// Parses the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The parsed value or the default value.</returns>
        public static DateTime ParseDateTime(string value, DateTime defaultValue)
        {
            DateTime dt;

            if (DateTime.TryParse(value, out dt))
            {
                return dt;
            }

            value = value.Trim();

            if (string.Equals(value, "now", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Now;
            }

            if (string.Equals(value, "today", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today;
            }

            if (string.Equals(value, "tomorrow", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today.AddDays(1);
            }

            if (string.Equals(value, "yesterday", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today.AddDays(-1);
            }

            Match m;

            if ((m = _inXUnits.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                string unit = m.Groups[2].Value;
                if (string.Equals(unit, "day", StringComparison.OrdinalIgnoreCase))
                {
                    return AddUnits(unit, DateTime.Today, x, defaultValue);
                }
                return AddUnits(unit, DateTime.Now, x, defaultValue);
            }

            if ((m = _XUnitsAgo.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                string unit = m.Groups[2].Value;
                if (string.Equals(unit, "day", StringComparison.OrdinalIgnoreCase))
                {
                    return AddUnits(unit, DateTime.Today, -x, defaultValue);
                }
                return AddUnits(unit, DateTime.Now, -x, defaultValue);
            }

            if ((m = _XUnitsFromNow.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                string unit = m.Groups[2].Value;
                return AddUnits(unit, DateTime.Now, x, defaultValue);
            }

            if ((m = _XDaysFromToday.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                return DateTime.Today.AddDays(x);
            }

            return defaultValue;
        }

        private static DateTime AddUnits(string unit, DateTime dt, int x, DateTime defaultValue)
        {
            if (string.Equals(unit, "second", StringComparison.OrdinalIgnoreCase))
            {
                return dt.AddSeconds(x);
            }

            if (string.Equals(unit, "minute", StringComparison.OrdinalIgnoreCase))
            {
                return dt.AddMinutes(x);
            }

            if (string.Equals(unit, "hour", StringComparison.OrdinalIgnoreCase))
            {
                return dt.AddHours(x);
            }

            if (string.Equals(unit, "day", StringComparison.OrdinalIgnoreCase))
            {
                return dt.AddDays(x);
            }

            if (string.Equals(unit, "month", StringComparison.OrdinalIgnoreCase))
            {
                return dt.AddMonths(x);
            }

            if (string.Equals(unit, "year", StringComparison.OrdinalIgnoreCase))
            {
                return dt.AddYears(x);
            }

            return defaultValue;
        }
    }
}
