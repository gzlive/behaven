using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN
{
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
            _regex = new Regex("^" + PatternMaker.GetPattern(_methodInfo) + "$", RegexOptions.IgnoreCase);
        }

        private object _target;
        private MethodInfo _methodInfo;
        private Regex _regex;

        internal bool TryExecute(Step step)
        {
            Match m = GetMatch(step);

            if (!m.Success)
                return false;

            object[] parameters = GetParametersForMethodFromMatch(m, step.Block);

            Invoke(parameters);

            AssertOnOutputParameters(m, step.Block, parameters, ref step.Text);

            return true;
        }

        internal bool Matches(Step step)
        {
            return GetMatch(step).Success;
        }

        private Regex firstWordReplacer = new Regex(@"^\S+");

        private Match GetMatch(Step step)
        {
            string text = firstWordReplacer.Replace(step.Text, step.Keyword);
            return _regex.Match(text);
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
                        if (block != null && BlockTypes.BlockTypeExistsFor(pi.ParameterType))
                        {
                            BlockType blockType = BlockTypes.GetBlockTypeFor(pi.ParameterType);
                            parameters[i] = blockType.GetObject(pi.ParameterType, block);
                        }
                        else
                        {
                            if (match.Groups[pi.Name].Success)
                            {
                                string value = match.Groups[pi.Name].Value;
                                parameters[i] = ValueParser.ParseValue(value, pi.ParameterType);
                            }
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

        private void AssertOnOutputParameters(Match match, IBlock block, object[] parameters, ref string description)
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

                        if (block != null && !InlineTypes.InlineTypeExistsFor(type))
                        {
                            passed &= block.Check(parameters[i]);
                        }
                        else if (InlineTypes.InlineTypeExistsFor(type))
                        {
                            string expectedValue = match.Groups[pi.Name].Value;
                            string actualValue = parameters[i] != null ? parameters[i].ToString() : null;

                            if (expectedValue != string.Format("{0}", actualValue))
                            {
                                Match m = _regex.Match(description);
                                Group group = m.Groups[pi.Name];
                                description = description.Substring(0, group.Index)
                                              + string.Format("{0} (was {1})", expectedValue, actualValue)
                                              + description.Substring(group.Index + group.Length);
                                passed = false;
                            }
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