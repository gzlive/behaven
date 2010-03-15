// <copyright file="UndefinedStepDefinitionHelper.cs" company="Jason Diamond">
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

using System.Collections.Generic;
using System.Text;

namespace BehaveN
{
    internal static class UndefinedStepDefinitionHelper
    {
        internal static string GetUndefinedStepCode(Step undefinedStep)
        {
            string methodName = GetMethodName(undefinedStep);
            string parameters = GetParameters(undefinedStep.Text, undefinedStep.Block);

            return string.Format(@"public void {0}({1})
{{
    throw new NotImplementedException();
}}", methodName, parameters);
        }

        private static string GetMethodName(Step step)
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            int arg = 1;

            foreach (string part in Split(step.Text))
            {
                if (sb.Length > 0)
                {
                    sb.Append("_");
                }

                if (IsInteger(part) || IsDecimal(part) || IsString(part))
                {
                    sb.AppendFormat("arg{0}", arg++);
                }
                else
                {
                    if (i == 0)
                        sb.Append(step.Type.ToString());
                    else
                        sb.Append(part);
                }

                i++;
            }

            return sb.ToString().ToLowerInvariant();
        }

        private static string GetParameters(string description, IBlock block)
        {
            List<string> parameters = new List<string>();

            int i = 1;

            foreach (string part in Split(description))
            {
                if (IsInteger(part))
                {
                    parameters.Add(string.Format("int arg{0}", i++));
                }
                else if (IsDecimal(part))
                {
                    parameters.Add(string.Format("decimal arg{0}", i++));
                }
                else if (IsString(part))
                {
                    parameters.Add(string.Format("string arg{0}", i++));
                }
            }

            if (block != null)
            {
                parameters.Add(string.Format("{0} {1}", block.GetSuggestedParameterType(), block.GetSuggestedParameterName()));
            }

            return string.Join(", ", parameters.ToArray());
        }

        private static string[] Split(string description)
        {
            List<string> parts = new List<string>();

            bool quoted = false;
            int start = 0;

            for (int i = 0; i < description.Length; i++)
            {
                char c = description[i];

                if (c == '\"') quoted = !quoted;

                if (!quoted && c == ' ')
                {
                    if (start < i)
                        parts.Add(description.Substring(start, i - start));

                    start = i + 1;
                }
            }

            parts.Add(description.Substring(start));

            return parts.ToArray();
        }

        private static bool IsInteger(string part)
        {
            int i;
            return int.TryParse(part, out i);
        }

        private static bool IsDecimal(string part)
        {
            decimal d;
            if (decimal.TryParse(part, out d))
                return true;
            if (part.Length >= 2 && part[0] == '$')
                return decimal.TryParse(part.Substring(1), out d);
            return false;
        }

        private static bool IsString(string part)
        {
            return part.Length > 2 && part[0] == '\"' && part[part.Length - 1] == '\"';
        }
    }
}
