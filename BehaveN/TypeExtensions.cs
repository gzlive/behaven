// <copyright file="TypeExtensions.cs" company="Jason Diamond">
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

namespace BehaveN
{
    using System;
    using System.Collections.Generic;

    internal static class TypeExtensions
    {
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Gets the type of the collection item.
        /// </summary>
        /// <param name="type">The type of items contained by the collection.</param>
        /// <returns>The collection item type.</returns>
        public static Type GetCollectionItemType(Type type)
        {
            if (!type.IsGenericType)
            {
                return null;
            }

            Type genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(IEnumerable<>) ||
                genericType == typeof(IList<>) ||
                genericType == typeof(ICollection<>) ||
                genericType == typeof(List<>) ||
                IsSetType(type))
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }

        public static bool IsSetType(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            Type genericType = type.GetGenericTypeDefinition();

            if (genericType == null)
            {
                return false;
            }

            // We have to use strings since these types are new in 3.5 and BehaveN is compiled for 2.0.
            return genericType.FullName == "System.Collections.Generic.HashSet`1" ||
                   genericType.FullName == "System.Collections.Generic.SortedSet`1";

            // TODO: It would be nice if we also checked the implemented interfaces to see if ISet<T> is there.
        }
    }
}
