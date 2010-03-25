// <copyright file="NameComparer.cs" company="Jason Diamond">
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
    using System.Text;

    /// <summary>
    /// A utility class for comparing member names.
    /// </summary>
    public static class NameComparer
    {
        /// <summary>
        /// Normalizes the name.
        /// </summary>
        /// <param name="name">The name to normalize.</param>
        /// <returns>The normalized name.</returns>
        public static string NormalizeName(string name)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in name)
            {
                if (Char.IsLetterOrDigit(c) || c == '_')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines if two normalized names are equal.
        /// </summary>
        /// <param name="normalizedName1">The first normalized name.</param>
        /// <param name="normalizedName2">The second normalized name.</param>
        /// <returns>True if the names are equal.</returns>
        public static bool NormalizedNamesAreEqual(string normalizedName1, string normalizedName2)
        {
            return string.Equals(normalizedName1, normalizedName2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
