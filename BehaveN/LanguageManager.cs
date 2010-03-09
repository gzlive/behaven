using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace BehaveN
{
    /// <summary>
    /// Manages getting strings for native languages.
    /// </summary>
    public class LanguageManager
    {
        private ResourceManager _en = new ResourceManager("BehaveN.Languages.en", typeof(LanguageManager).Assembly);
        private Dictionary<string, ResourceManager> _languages = new Dictionary<string, ResourceManager>();

        /// <summary>
        /// Gets the string with the specified name in the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="name">The name.</param>
        /// <returns>The string.</returns>
        public string GetString(string language, string name)
        {
            ResourceManager rm;

            if (!_languages.ContainsKey(language))
            {
                rm = new ResourceManager("BehaveN.Languages." + language, typeof(LanguageManager).Assembly);

                try
                {
                    rm.GetString("x");

                    _languages[language] = rm;
                }
                catch
                {
                    rm = _en;
                }
            }
            else
            {
                rm = _languages[language];
            }

            return rm.GetString(name) ?? _en.GetString(name);
        }
    }
}
