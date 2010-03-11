// <copyright file="NameParser.cs" company="Jason Diamond">
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
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// A utility class for parsing method names.
    /// </summary>
    public static class NameParser
    {
        private static readonly Regex _stepDefinitionTester = new Regex(@"^([Gg]iven|[Ww]hen|[Tt]hen)(_|[A-Z])");
        private static readonly Regex _underscoreSplitter = new Regex(@"_+");
        private static readonly Regex _camelCaseSplitter = new Regex(@"(?<!^)(?=[A-Z])");

        /// <summary>
        /// Determines if the specified method is a step definition.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns><c>true</c> if the method name starts with given, when, or then; otherwise <c>false</c></returns>
        public static bool IsStepDefinition(MethodInfo methodInfo)
        {
            return _stepDefinitionTester.IsMatch(methodInfo.Name)
                || Attribute.IsDefined(methodInfo, typeof(StepAttribute));
        }

        /// <summary>
        /// Parses the specified method's name.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns>The parsed name.</returns>
        public static string Parse(MethodInfo methodInfo)
        {
            return Parse(methodInfo.Name);
        }

        /// <summary>
        /// Parses the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The parsed name.</returns>
        public static string Parse(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            string result = name;

            if (result.Contains("_"))
            {
                result = string.Join(" ", _underscoreSplitter.Split(result.Trim('_')));
            }
            else
            {
                result = string.Join(" ", _camelCaseSplitter.Split(result));
            }

            return result;
        }
    }
}
