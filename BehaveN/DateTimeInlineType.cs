using System;

namespace BehaveN
{
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
}