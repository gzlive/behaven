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
            List<string> splits = new List<string>(NameParser.Parse(methodInfo).Split());

            foreach (var pi in methodInfo.GetParameters())
            {
                if (InlineTypes.InlineTypeExistsFor(pi.ParameterType))
                {
                    int parameterIndex = splits.FindIndex(delegate(string s) { return s.Equals(pi.Name, StringComparison.OrdinalIgnoreCase); });

                    if (parameterIndex != -1)
                    {
                        splits[parameterIndex] = GetPatternForParameter(pi);
                    }
                }
            }

            return string.Format(@"\s*{0}\s*", string.Join(@"\s+", splits.ToArray()));
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
