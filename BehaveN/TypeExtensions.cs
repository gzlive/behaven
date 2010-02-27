using System;
using System.Collections.Generic;
using System.Text;

namespace BehaveN
{
    internal static class TypeExtensions
    {
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
