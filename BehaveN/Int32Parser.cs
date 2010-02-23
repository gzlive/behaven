using System;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Utility class that parses Int32 strings.
    /// </summary>
    public static class Int32Parser
    {
        private static readonly Regex _ordinalRegex = new Regex(@"(\d+)(?:st|nd|rd|th)", RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The parsed value or 0 if it failed to parse.</returns>
        public static int ParseInt32(string value)
        {
            return ParseInt32(value, 0);
        }

        /// <summary>
        /// Parses the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The parsed value or the default value.</returns>
        public static int ParseInt32(string value, int defaultValue)
        {
            int i;

            if (int.TryParse(value, out i))
            {
                return i;
            }

            Match m = _ordinalRegex.Match(value);

            if (m.Success)
            {
                return int.Parse(m.Groups[1].Value);
            }

            return defaultValue;
        }
    }
}
