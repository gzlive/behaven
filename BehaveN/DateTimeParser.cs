// <copyright file="DateTimeParser.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

namespace BehaveN
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Utility class that parses DateTime strings.
    /// </summary>
    public static class DateTimeParser
    {
        /// <summary>
        /// Regex that matches "in 3 days".
        /// </summary>
        private static readonly Regex InXUnits = new Regex(@"in\s+(\d+)\s+(.+?)s?\b", RegexOptions.IgnoreCase);

        /// <summary>
        /// Regex that matches "3 days ago".
        /// </summary>
        private static readonly Regex XUnitsAgo = new Regex(@"(\d+)\s+(.+?)s?\s+ago", RegexOptions.IgnoreCase);

        /// <summary>
        /// Regex that matches "3 days from now".
        /// </summary>
        private static readonly Regex XUnitsFromNow = new Regex(@"(\d+)\s+(.+?)s?\s+from\s+now", RegexOptions.IgnoreCase);

        /// <summary>
        /// Regex that matches "3 days from today".
        /// </summary>
        private static readonly Regex XDaysFromToday = new Regex(@"(\d+)\s+days?\s+from\s+today", RegexOptions.IgnoreCase);

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

            if ((m = InXUnits.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                string unit = m.Groups[2].Value;
                
                if (string.Equals(unit, "day", StringComparison.OrdinalIgnoreCase))
                {
                    return AddUnits(DateTime.Today, unit, x, defaultValue);
                }

                return AddUnits(DateTime.Now, unit, x, defaultValue);
            }

            if ((m = XUnitsAgo.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                string unit = m.Groups[2].Value;
                
                if (string.Equals(unit, "day", StringComparison.OrdinalIgnoreCase))
                {
                    return AddUnits(DateTime.Today, unit, -x, defaultValue);
                }
                
                return AddUnits(DateTime.Now, unit, -x, defaultValue);
            }

            if ((m = XUnitsFromNow.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                string unit = m.Groups[2].Value;
                return AddUnits(DateTime.Now, unit, x, defaultValue);
            }

            if ((m = XDaysFromToday.Match(value)).Success)
            {
                int x = int.Parse(m.Groups[1].Value);
                return DateTime.Today.AddDays(x);
            }

            return defaultValue;
        }

        /// <summary>
        /// Adds the specified units to the a DateTime.
        /// </summary>
        /// <param name="dt">The DateTime to add to.</param>
        /// <param name="unit">The unit type.</param>
        /// <param name="x">The number of units to add.</param>
        /// <param name="defaultValue">The default value in case the units is invalid.</param>
        /// <returns>The new DateTime.</returns>
        private static DateTime AddUnits(DateTime dt, string unit, int x, DateTime defaultValue)
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
