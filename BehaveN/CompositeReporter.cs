// <copyright file="CompositeReporter.cs" company="Jason Diamond">
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
    using System.Collections.Generic;

    /// <summary>
    /// Represents a reporter that can report to one or more child reporters.
    /// </summary>
    public class CompositeReporter : Reporter
    {
        /// <summary>
        /// The list of child reporters.
        /// </summary>
        private readonly List<Reporter> children = new List<Reporter>();

        /// <summary>
        /// Gets the count of reporters.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return this.children.Count; }
        }

        /// <summary>
        /// Reports the specifications file.
        /// </summary>
        /// <param name="specificationsFile">The specifications file.</param>
        /// <remarks>This reports all scenarios in the file and their
        /// undefined steps.</remarks>
        public override void ReportSpecificationsFile(SpecificationsFile specificationsFile)
        {
            foreach (var reporter in this.children)
            {
                reporter.ReportSpecificationsFile(specificationsFile);
            }
        }

        /// <summary>
        /// Reports the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public override void ReportScenario(Scenario scenario)
        {
            foreach (var reporter in this.children)
            {
                reporter.ReportScenario(scenario);
            }
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        /// <param name="undefinedSteps">The undefined steps.</param>
        public override void ReportUndefinedSteps(ICollection<Step> undefinedSteps)
        {
            foreach (var reporter in this.children)
            {
                reporter.ReportUndefinedSteps(undefinedSteps);
            }
        }

        /// <summary>
        /// Adds the specified reporter.
        /// </summary>
        /// <param name="reporter">The reporter.</param>
        public void Add(Reporter reporter)
        {
            this.children.Add(reporter);
        }

        /// <summary>
        /// Removes the reporter at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            this.children.RemoveAt(index);
        }
    }
}
