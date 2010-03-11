// <copyright file="ValueFormatter.cs" company="Jason Diamond">
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BehaveN
{
    /// <summary>
    /// Utility methods to format values.
    /// </summary>
    public static class ValueFormatter
    {
        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The formatted value.</returns>
        public static string FormatValue(object value)
        {
            if (value != null)
            {
                Type type = value.GetType();

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    StringBuilder sb = new StringBuilder();

                    bool first = true;

                    foreach (object item in (IEnumerable)value)
                    {
                        if (!first)
                        {
                            sb.Append(", ");
                        }

                        sb.AppendFormat("{0}", item);

                        first = false;
                    }

                    return sb.ToString();
                }
            }

            return string.Format("{0}", value);
        }
    }
}