// <copyright file="InlineTypes.cs" company="Jason Diamond">
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
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Helper methods for working with InlineType objects.
    /// </summary>
    public static class InlineTypes
    {
        static InlineTypes()
        {
            List<Type> types = new List<Type>(typeof(InlineTypes).Assembly.GetTypes());
            types = types.FindAll(delegate(Type t) { return typeof(InlineType).IsAssignableFrom(t) && !t.IsAbstract; });
            inlineTypes = types.ConvertAll(delegate(Type t) { return (InlineType)Activator.CreateInstance(t); });
        }

        private static List<InlineType> inlineTypes;

        /// <summary>
        /// Gets the inline types.
        /// </summary>
        /// <returns>A list of InlineType objects.</returns>
        public static List<InlineType> GetInlineTypes()
        {
            return inlineTypes;
        }

        /// <summary>
        /// Gets the inline type that handles the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The inline type.</returns>
        public static InlineType GetInlineTypeFor(Type type)
        {
            if (type.IsByRef)
                type = type.GetElementType();

            if (TypeExtensions.IsNullable(type))
                type = Nullable.GetUnderlyingType(type);

            return inlineTypes.Find(delegate(InlineType it) { return it.HandlesType(type); });
        }

        /// <summary>
        /// Determines if an inline type existis for the specific type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if an inline type exists.</returns>
        public static bool InlineTypeExistsFor(Type type)
        {
            return GetInlineTypeFor(type) != null;
        }
    }
}
