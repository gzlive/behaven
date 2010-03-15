// <copyright file="ValueParser.cs" company="Jason Diamond">
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

using System;
using System.Collections;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// A utility class for parsing values.
    /// </summary>
    public static class ValueParser
    {
        /// <summary>
        /// Parses the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The actual value.</returns>
        public static object ParseValue(string value, Type type)
        {
            if (value == null)
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }

                return null;
            }

            if (TypeExtensions.IsNullable(type))
            {
                if (value == "null")
                {
                    return null;
                }

                type = Nullable.GetUnderlyingType(type);
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, NameComparer.NormalizeName(value), true);
            }

            if (type == typeof(int))
            {
                return Int32Parser.ParseInt32(value);
            }

            if (type == typeof(DateTime))
            {
                return DateTimeParser.ParseDateTime(value);
            }

            Type itemType = BlockType.GetCollectionItemType(type);

            if (itemType != null)
            {
                IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

                string[] splits = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string split in splits)
                {
                    list.Add(ParseValue(split.Trim(), itemType));
                }

                return list;
            }

            return Convert.ChangeType(value, type);
        }
    }
}
