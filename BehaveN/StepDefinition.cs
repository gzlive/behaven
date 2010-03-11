// <copyright file="StepDefinition.cs" company="Jason Diamond">
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
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents a step definition.
    /// </summary>
    [DebuggerDisplay("{_methodInfo}")]
    public class StepDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepDefinition"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="methodInfo">The method info.</param>
        public StepDefinition(object target, MethodInfo methodInfo)
        {
            _target = target;
            _methodInfo = methodInfo;
            _regexes = new List<Regex>();
            _regexes.Add(new Regex("^" + PatternMaker.GetPattern(_methodInfo) + "$", RegexOptions.IgnoreCase));

            foreach (StepAttribute stepAttribute in Attribute.GetCustomAttributes(_methodInfo, typeof(StepAttribute)))
            {
                _regexes.Add(new Regex("^" + PatternMaker.GetPattern(stepAttribute.Text, _methodInfo) + "$", RegexOptions.IgnoreCase));
            }
        }

        private object _target;
        private MethodInfo _methodInfo;
        private List<Regex> _regexes;

        internal bool TryExecute(Step step)
        {
            Match m = GetMatch(step);

            if (!m.Success)
                return false;

            object[] parameters = GetParametersForMethodFromMatch(m, step.Block);

            Invoke(parameters);

            CheckOutputParameters(m, step, parameters);

            return true;
        }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName
        {
            get { return _methodInfo.Name; }
        }

        /// <summary>
        /// Determines if the step definition matches the specified step.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns>True or false.</returns>
        public bool Matches(Step step)
        {
            return GetMatch(step).Success;
        }

        private Regex firstWordReplacer = new Regex(@"^\S+");

        private Match GetMatch(Step step)
        {
            string text = firstWordReplacer.Replace(step.Text, step.Type.ToString());

            foreach (var regex in _regexes)
            {
                Match m = regex.Match(text);

                if (m.Success)
                    return m;
            }

            return Match.Empty;
        }

        private object[] GetParametersForMethodFromMatch(Match match, IBlock block)
        {
            object[] parameters = null;

            if (_methodInfo.GetParameters().Length > 0)
            {
                parameters = new object[_methodInfo.GetParameters().Length];

                int i = 0;

                foreach (ParameterInfo pi in _methodInfo.GetParameters())
                {
                    if (!pi.IsOut)
                    {
                        if (InlineTypes.InlineTypeExistsFor(pi.ParameterType))
                        {
                            if (match.Groups[pi.Name].Success)
                            {
                                string value = match.Groups[pi.Name].Value;
                                parameters[i] = ValueParser.ParseValue(value, pi.ParameterType);
                            }
                        }
                        else if (block != null && BlockTypes.BlockTypeExistsFor(pi.ParameterType))
                        {
                            BlockType blockType = BlockTypes.GetBlockTypeFor(pi.ParameterType);
                            parameters[i] = blockType.GetObject(pi.ParameterType, block);
                        }
                    }

                    i++;
                }
            }

            return parameters;
        }

        private void Invoke(object[] parameters)
        {
            _methodInfo.Invoke(_target, parameters);
        }

        private void CheckOutputParameters(Match match, Step step, object[] parameters)
        {
            bool passed = true;

            if (_methodInfo.GetParameters().Length > 0)
            {
                int i = 0;

                foreach (ParameterInfo pi in _methodInfo.GetParameters())
                {
                    if (pi.IsOut)
                    {
                        Type type = pi.ParameterType.GetElementType();

                        if (InlineTypes.InlineTypeExistsFor(type))
                        {
                            object expectedValue = ValueParser.ParseValue(match.Groups[pi.Name].Value, type);
                            object actualValue = parameters[i];

                            if (!object.Equals(actualValue, expectedValue))
                            {
                                Match m = GetMatch(step);
                                Group group = m.Groups[pi.Name];
                                step.Text = step.Text.Substring(0, group.Index)
                                              + string.Format("{0} (was {1})", expectedValue, actualValue)
                                              + step.Text.Substring(group.Index + group.Length);
                                passed = false;
                            }
                        }
                        else if (step.Block != null && BlockTypes.BlockTypeExistsFor(type))
                        {
                            passed &= step.Block.Check(parameters[i]);
                        }
                    }

                    i++;
                }
            }

            if (!passed)
            {
                throw new VerificationException(new Exception("One or more output parameters did not pass."));
            }
        }
    }
}