using System;
using System.Collections.Generic;
using System.IO;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter that can report to one or more child reporters.
    /// </summary>
    public class DefaultReporter : CompositeReporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultReporter"/> class.
        /// </summary>
        public DefaultReporter()
        {
            Add(new PlainTextReporter(Console.Out));
        }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>The destination.</value>
        /// <remarks>This is used by reporters that report to files.</remarks>
        public override string Destination
        {
            set
            {
                base.Destination = value;

                if (Count > 1)
                {
                    RemoveAt(1);
                }

                Add(new HtmlReporter(new StreamWriter(value)));
            }
        }
    }
}
