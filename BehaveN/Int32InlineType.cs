// <copyright file="Int32InlineType.cs" company="Jason Diamond">
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

    /// <summary>
    /// Inline type for Int32 values.
    /// </summary>
    internal class Int32InlineType : InlineType
    {
        /// <summary>
        /// Determines if this type handles the specified type.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <returns>
        /// true if this type handles the specified type
        /// </returns>
        public override bool HandlesType(Type type)
        {
            return type == typeof(int);
        }

        /// <summary>
        /// Gets the pattern for the specified type.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <returns>The pattern.</returns>
        public override string GetPattern(Type type)
        {
            if (TypeExtensions.IsNullable(type))
            {
                return @"(?<{0}>(?:-?\d+)|(?:null))(?:st|nd|rd|th)?";
            }

            return @"(?<{0}>-?\d+)(?:st|nd|rd|th)?";
        }
    }
}