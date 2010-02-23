using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents a step definition.
    /// </summary>
    public class StepMethod
    {
        private readonly object _target;
        private readonly MethodInfo _methodInfo;
        private readonly StepType _stepType;
        private readonly Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepMethod"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="stepType">The step type.</param>
        /// <param name="regex">The regex.</param>
        public StepMethod(object target, MethodInfo methodInfo, StepType stepType, Regex regex)
        {
            _target = target;
            _methodInfo = methodInfo;
            _stepType = stepType;
            _regex = regex;
        }

        /// <summary>
        /// Invokes if matching.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        /// <returns>Whether the step method was invoked or not.</returns>
        public bool InvokeIfMatching(ref string description, StepType stepType, IConvertibleObject convertibleObject)
        {
            Match m = Match(description, stepType);

            if (m != null && m.Success)
            {
                object[] parameters = GetParametersForMethodFromMatch(m, convertibleObject);

                Invoke(parameters);

                AssertOnOutputParameters(m, convertibleObject, parameters, ref description);

                return true;
            }

            return false;
        }

        internal bool Matches(StepType stepType, string description, IConvertibleObject convertibleObject)
        {
            Match m = Match(description, stepType);

            return m != null && m.Success;
        }

        private Match Match(string description, StepType stepType)
        {
            if (_stepType != StepType.Unspecified && _stepType != stepType)
            {
                return null;
            }

            return _regex.Match(description);
        }

        private object[] GetParametersForMethodFromMatch(Match match, IConvertibleObject convertibleObject)
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
                        if (convertibleObject != null && !InlineTypes.InlineTypeExistsFor(pi.ParameterType))
                        {
                            Type itemType = GetCollectionItemType(pi.ParameterType);

                            if (itemType != null)
                            {
                                parameters[i] = convertibleObject.ToList(itemType);
                            }
                            else
                            {
                                parameters[i] = convertibleObject.ToObject(pi.ParameterType);
                            }
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

        private void AssertOnOutputParameters(Match match, IConvertibleObject convertibleObject, object[] parameters, ref string description)
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

                        if (convertibleObject != null && !InlineTypes.InlineTypeExistsFor(type))
                        {
                            passed &= convertibleObject.Check(parameters[i]);
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

        internal static Type GetCollectionItemType(Type parameterType)
        {
            if (!parameterType.IsGenericType) return null;

            Type genericType = parameterType.GetGenericTypeDefinition();

            if (genericType == typeof(IEnumerable<>) ||
                genericType == typeof(IList<>) ||
                genericType == typeof(ICollection<>) ||
                genericType == typeof(List<>))
            {
                return parameterType.GetGenericArguments()[0];
            }

            return null;
        }

        private void Invoke(object[] parameters)
        {
            _methodInfo.Invoke(_target, parameters);
        }
    }
}
