using System;

namespace BehaveN
{
    internal class DecimalInlineType : InlineType
    {
        public override bool HandlesType(Type type)
        {
            return type == typeof(decimal);
        }

        public override string GetPattern(Type type)
        {
            if (TypeExtensions.IsNullable(type))
                return @"(?:\$\s*)?(?<{0}>(?:-?\d+(?:\.\d+)?)|(?:null))";

            return @"(?:\$\s*)?(?<{0}>-?\d+(?:\.\d+)?)";
        }
    }
}