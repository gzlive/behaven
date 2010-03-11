// <copyright file="DefaultReporter.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
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
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents the default reporter.
    /// </summary>
    /// <remarks>The default reporter always reports to the console. It's
    /// <see cref="Reporter.Destination" /> property will create and use another
    /// reporter (based on the file extension of the new destination).</remarks>
    public class DefaultReporter : Reporter
    {
        /// <summary>
        /// The composite reporter that holds the reporter that writes
        /// to the console and the other reporter that writes to a file.
        /// </summary>
        private readonly CompositeReporter compositeReporter = new CompositeReporter();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultReporter"/> class.
        /// </summary>
        public DefaultReporter()
        {
            this.compositeReporter.Add(new PlainTextReporter(Console.Out));
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

                if (this.compositeReporter.Count > 1)
                {
                    this.compositeReporter.RemoveAt(1);
                }

                this.compositeReporter.Add(GetReporterBasedOnExtension(value, new StreamWriter(value)));
            }
        }

        /// <summary>
        /// Reports the specifications file.
        /// </summary>
        /// <param name="specificationsFile">The specifications file.</param>
        /// <remarks>This reports all scenarios in the file and their
        /// undefined steps.</remarks>
        public override void ReportSpecificationsFile(SpecificationsFile specificationsFile)
        {
            this.compositeReporter.ReportSpecificationsFile(specificationsFile);
        }

        /// <summary>
        /// Reports the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public override void ReportScenario(Scenario scenario)
        {
            this.compositeReporter.ReportScenario(scenario);
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        /// <param name="undefinedSteps">The undefined steps.</param>
        public override void ReportUndefinedSteps(ICollection<Step> undefinedSteps)
        {
            this.compositeReporter.ReportUndefinedSteps(undefinedSteps);
        }
    }
}
