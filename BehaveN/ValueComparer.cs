using System;

namespace BehaveN
{
    /// <summary>
    /// Utility methods to compare values.
    /// </summary>
    public static class ValueComparer
    {
        /// <summary>
        /// Compares the values.
        /// </summary>
        /// <param name="actual">The actual.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="type">The type.</param>
        /// <returns>True or false.</returns>
        public static bool CompareValues(string actual, string expected, Type type)
        {
            if (type.IsEnum || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].IsEnum))
            {
                actual = NameComparer.NormalizeName(actual);
                expected = NameComparer.NormalizeName(expected);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                expected = ValueFormatter.FormatValue(DateTimeParser.ParseDateTime(expected));
            }

            return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
        }
    }
}
