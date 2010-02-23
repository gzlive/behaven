using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Represents an inline parameter type.
    /// </summary>
    public abstract class InlineType
    {
        /// <summary>
        /// Determines if this type handles the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true if this type handles the specified type</returns>
        public abstract bool HandlesType(Type type);

        /// <summary>
        /// Gets the pattern for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The pattern.</returns>
        public abstract string GetPattern(Type type);
    }

    internal class StringInlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type == typeof(string);
        }

        public override string GetPattern(Type type)
        {
            return @"\""?(?<{0}>.+?)\""?";
        }
    }

    internal class Int32InlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type == typeof(int);
        }

        public override string GetPattern(Type type)
        {
            return @"(?<{0}>-?\d+)(?:st|nd|rd|th)?";
        }
    }

    internal class DecimalInlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type == typeof(decimal);
        }

        public override string GetPattern(Type type)
        {
            return @"(?:\$\s*)?(?<{0}>-?\d+(?:\.\d+)?)";
        }
    }

    internal class DateTimeInlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type == typeof(DateTime);
        }

        public override string GetPattern(Type type)
        {
            return @"\""?(?<{0}>.+?)\""?";
        }
    }

    internal class EnumInlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type.IsEnum;
        }

        public override string GetPattern(Type type)
        {
            List<string> subPatterns = new List<string>();

            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string parsed = NameParser.Parse(fieldInfo.Name, false);
                subPatterns.Add("(?:" + string.Join(@"\s*", parsed.Split()) + ")");
            }

            return "(?<{0}>" + string.Join("|", subPatterns.ToArray()) + ")";
        }
    }
}
