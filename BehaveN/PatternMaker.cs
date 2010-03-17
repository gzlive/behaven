// <copyright file="PatternMaker.cs" company="Jason Diamond">
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
    /// Generates regular expression patterns for a method.
    /// </summary>
    public static class PatternMaker
    {
        /// <summary>
        /// Gets the pattern for a method.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns>A regular expression pattern.</returns>
        public static string GetPattern(MethodInfo methodInfo)
        {
            return GetPattern(methodInfo.Name, methodInfo);
        }

        internal static string GetPattern(string text, MethodInfo methodInfo)
        {
            var splits = new List<string>(NameParser.Parse(text).Split());

            ReplaceArgPlaceholdersWithPatterns(methodInfo, splits);

            return string.Format(@"\s*{0}\s*", string.Join(@"\s+", splits.ToArray()));
        }

        private static void ReplaceArgPlaceholdersWithPatterns(MethodInfo methodInfo, List<string> splits)
        {
            int i = 1;

            foreach (var pi in methodInfo.GetParameters())
            {
                if (InlineTypes.InlineTypeExistsFor(pi.ParameterType))
                {
                    string argPlaceholder = "arg" + i;

                    int parameterIndex = splits.FindIndex(delegate(string s) { return s.Equals(argPlaceholder, StringComparison.OrdinalIgnoreCase); });

                    if (parameterIndex != -1)
                    {
                        splits[parameterIndex] = GetPatternForParameter(pi);
                    }
                }

                i++;
            }
        }

        private static string GetPatternForParameter(ParameterInfo parameterInfo)
        {
            Type type = parameterInfo.ParameterType;

            if (parameterInfo.IsOut)
            {
                type = parameterInfo.ParameterType.GetElementType();
            }

            InlineType inlineType = InlineTypes.GetInlineTypeFor(type);

            if (inlineType != null)
            {
                return string.Format(inlineType.GetPattern(type), parameterInfo.Name);
            }

            return null;
        }
    }
}
