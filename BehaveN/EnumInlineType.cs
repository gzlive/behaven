using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaveN
{
    internal class EnumInlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type.IsEnum;
        }

        public override string GetPattern(Type type)
        {
            List<string> subPatterns = new List<string>();

            if (TypeExtensions.IsNullable(type))
            {
                type = Nullable.GetUnderlyingType(type);
                subPatterns.Add("(?:null)");
            }

            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string parsed = NameParser.Parse(fieldInfo.Name);
                subPatterns.Add("(?:" + string.Join(@"\s*", parsed.Split()) + ")");
            }

            return "(?<{0}>" + string.Join("|", subPatterns.ToArray()) + ")";
        }
    }
}