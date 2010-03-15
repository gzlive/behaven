// <copyright file="Int32Parser.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
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
