// <copyright file="ValueGetter.cs" company="Jason Diamond">
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
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Utility class that gets values from objects. Or, will be soon
    /// For now, it just gets PropertyInfos from Types.
    /// </summary>
    public static class ValueGetter
    {
        ///<summary>
        /// Gets a PropertyInfo by its name.
        ///</summary>
        ///<param name="type">The type.</param>
        ///<param name="propertyName">The property name.</param>
        ///<returns>The property info.</returns>
        /// <remarks>
        /// This works around a bug in the .NET reflection API. It was fixed in .NET 4.
        /// See https://connect.microsoft.com/VisualStudio/feedback/details/490003/getproperty-throws-when-property-overrides-generic-base-class-property
        /// </remarks>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            var properties = new List<PropertyInfo>(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            var types = GetFlattenedTypeHierarchy(type);

            SortPropertiesByDeclaringType(properties, types);

            foreach (var pi in properties)
            {
                if (string.Equals(pi.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return pi;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a list of types the specified type derives from or implements.
        /// </summary>
        /// <param name="type">The type to start with.</param>
        /// <returns>The list of types.</returns>
        private static List<Type> GetFlattenedTypeHierarchy(Type type)
        {
            var types = new List<Type>();
            GetFlattenedTypeHierarchyHelper(type, types);
            return types;
        }

        /// <summary>
        /// Uses recursion to get all base types including interface types.
        /// </summary>
        /// <param name="type">The current type to add to the list.</param>
        /// <param name="types">The list of types to add to.</param>
        private static void GetFlattenedTypeHierarchyHelper(Type type, List<Type> types)
        {
            if (type != null)
            {
                types.Add(type);

                foreach (var interfaceType in type.GetInterfaces())
                {
                    GetFlattenedTypeHierarchyHelper(interfaceType, types);
                }

                GetFlattenedTypeHierarchyHelper(type.BaseType, types);
            }
        }

        /// <summary>
        /// Sorts the properties by the position of their declaring type in the type hierarchy.
        /// </summary>
        /// <param name="properties">The list of properties to sort.</param>
        /// <param name="types">The flattened type hierarchy.</param>
        private static void SortPropertiesByDeclaringType(List<PropertyInfo> properties, List<Type> types)
        {
            properties.Sort((a, b) => types.IndexOf(a.DeclaringType).CompareTo(types.IndexOf(b.DeclaringType)));
        }
    }
}