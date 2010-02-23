using System;
using System.Text;

namespace BehaveN
{
    /// <summary>
    /// A utility class for comparing member names.
    /// </summary>
    public static class NameComparer
    {
        /// <summary>
        /// Normalizes the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The normalized name.</returns>
        public static string NormalizeName(string name)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in name)
            {
                if (Char.IsLetterOrDigit(c) || c == '_')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines if two normalized names are equal.
        /// </summary>
        /// <param name="normalizedName1">The first normalized name.</param>
        /// <param name="normalizedName2">The second normalized name.</param>
        /// <returns></returns>
        public static bool NormalizedNamesAreEqual(string normalizedName1, string normalizedName2)
        {
            return string.Equals(normalizedName1, normalizedName2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
