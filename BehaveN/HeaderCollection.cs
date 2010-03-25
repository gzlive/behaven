using System;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a collection of headers.
    /// </summary>
    public class HeaderCollection
    {
        private readonly Dictionary<string, string> values = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified name.
        /// </summary>
        /// <value></value>
        public string this[string name]
        {
            get
            {
                string value;

                if (this.values.TryGetValue(name, out value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                this.values[name] = value;
            }
        }
    }
}