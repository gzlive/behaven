using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BehaveN
{
    /// <summary>
    /// Utility methods to format values.
    /// </summary>
    public static class ValueFormatter
    {
        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The formatted value.</returns>
        public static string FormatValue(object value)
        {
            if (value != null)
            {
                Type type = value.GetType();

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    StringBuilder sb = new StringBuilder();

                    bool first = true;

                    foreach (object item in (IEnumerable)value)
                    {
                        if (!first)
                        {
                            sb.Append(", ");
                        }

                        sb.AppendFormat("{0}", item);

                        first = false;
                    }

                    return sb.ToString();
                }
            }

            return string.Format("{0}", value);
        }
    }
}