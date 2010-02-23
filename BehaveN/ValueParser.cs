using System;
using System.Collections;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// A utility class for parsing values.
    /// </summary>
    public static class ValueParser
    {
        /// <summary>
        /// Parses the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The actual value.</returns>
        public static object ParseValue(string value, Type type)
        {
            if (value == null)
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }

                return null;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type innerType = type.GetGenericArguments()[0];

                type = innerType;
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, NameComparer.NormalizeName(value), true);
            }

            if (type == typeof(int))
            {
                return Int32Parser.ParseInt32(value);
            }

            if (type == typeof(DateTime))
            {
                return DateTimeParser.ParseDateTime(value);
            }

            Type itemType = StepMethod.GetCollectionItemType(type);

            if (itemType != null)
            {
                IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

                string[] splits = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string split in splits)
                {
                    list.Add(ParseValue(split.Trim(), itemType));
                }

                return list;
            }

            return Convert.ChangeType(value, type);
        }
    }
}
