// <copyright file="StringExtensions.cs" company="Jason Diamond">
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

using System.Text;
using System.Text.RegularExpressions;

namespace BehaveN
{
    using System.Collections.Generic;

    internal static class StringExtensions
    {
        private static readonly Regex punctuationRemover = new Regex(@"(?<=\p{L})\p{P}");

        public static string RemovePunctuationFromOutsideQuotedParts(string value)
        {
            string[] parts = SplitTokens(value);

            for (int i = 0; i < parts.Length; i++)
            {
                if (!parts[i].StartsWith("\""))
                {
                    parts[i] = punctuationRemover.Replace(parts[i], "");
                }
            }

            return string.Join(" ", parts);
        }

        public static string[] SplitTokens(string value)
        {
            var parts = new List<string>();

            bool quoted = false;
            int start = 0;

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                if (c == '\"') quoted = !quoted;

                if (!quoted && c == ' ')
                {
                    if (start < i)
                        parts.Add(value.Substring(start, i - start));

                    start = i + 1;
                }
            }

            parts.Add(value.Substring(start));

            return parts.ToArray();
        }
    }
}
