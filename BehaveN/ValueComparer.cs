// <copyright file="ValueComparer.cs" company="Jason Diamond">
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

using System;

namespace BehaveN
{
    /// <summary>
    /// Utility methods to compare values.
    /// </summary>
    public static class ValueComparer
    {
        /// <summary>
        /// Compares the values.
        /// </summary>
        /// <param name="actual">The actual.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="type">The type.</param>
        /// <returns>True or false.</returns>
        public static bool CompareValues(string actual, string expected, Type type)
        {
            if (type.IsEnum || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].IsEnum))
            {
                actual = NameComparer.NormalizeName(actual);
                expected = NameComparer.NormalizeName(expected);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                expected = ValueFormatter.FormatValue(DateTimeParser.ParseDateTime(expected));
            }

            return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
        }
    }
}
