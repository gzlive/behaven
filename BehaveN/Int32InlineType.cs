using System;

namespace BehaveN
{
    internal class Int32InlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type == typeof(int);
        }

        public override string GetPattern(Type type)
        {
            if (TypeExtensions.IsNullable(type))
                return @"(?<{0}>(?:-?\d+)|(?:null))(?:st|nd|rd|th)?";

            return @"(?<{0}>-?\d+)(?:st|nd|rd|th)?";
        }
    }
}