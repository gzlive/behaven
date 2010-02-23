using System;

namespace BehaveN
{
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
}