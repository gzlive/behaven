// <copyright file="LanguageManager.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

namespace BehaveN
{
    using System.Collections.Generic;
    using System.Resources;

    /// <summary>
    /// Manages getting strings for native languages.
    /// </summary>
    public class LanguageManager
    {
        /// <summary>
        /// The resource manager for English resources.
        /// </summary>
        private ResourceManager englishResources = new ResourceManager("BehaveN.Languages.en", typeof(LanguageManager).Assembly);

        /// <summary>
        /// The resource managers for other languages.
        /// </summary>
        private Dictionary<string, ResourceManager> otherResources = new Dictionary<string, ResourceManager>();

        /// <summary>
        /// Gets the string with the specified name in the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="name">The name of the string.</param>
        /// <returns>The actual string.</returns>
        public string GetString(string language, string name)
        {
            ResourceManager rm;

            if (!this.otherResources.ContainsKey(language))
            {
                rm = new ResourceManager("BehaveN.Languages." + language, typeof(LanguageManager).Assembly);

                try
                {
                    rm.GetString("x");

                    this.otherResources[language] = rm;
                }
                catch
                {
                    rm = this.englishResources;
                }
            }
            else
            {
                rm = this.otherResources[language];
            }

            return rm.GetString(name) ?? this.englishResources.GetString(name);
        }
    }
}
