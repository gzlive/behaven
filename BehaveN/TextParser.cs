using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BehaveN
{
    internal static class TextParser
    {
        private static readonly Regex _languageRegex = new Regex(@"#\s*language\s*:\s*(\S+)", RegexOptions.IgnoreCase);

        internal static string DiscoverLanguage(string text)
        {
            Match m = _languageRegex.Match(text);

            if (m.Success) return m.Groups[1].Value;

            return "en";
        }

        internal static List<string> GetLines(string text)
        {
            List<string> lines = new List<string>();

            using (StringReader reader = new StringReader(text))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (LineIsNotEmptyAndNotAComment(line))
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        private static bool LineIsNotEmptyAndNotAComment(string line)
        {
            return line != "" && !line.StartsWith("#");
        }
    }
}
