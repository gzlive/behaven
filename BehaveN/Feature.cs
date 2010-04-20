// <copyright file="Feature.cs" company="Jason Diamond">
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

using System.IO;
using System.Reflection;

namespace BehaveN
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a feature.
    /// </summary>
    public class Feature
    {
        private readonly HeaderCollection headers = new HeaderCollection();
        private readonly StepDefinitionCollection stepDefinitions = new StepDefinitionCollection();
        private readonly ScenarioCollection scenarios;
        private bool passed;
        private Reporter reporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        public Feature()
        {
            this.scenarios = new ScenarioCollection(this);
        }

        /// <summary>
        /// Reads the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="name">The name of the embedded resource.</param>
        public void ReadEmbeddedResource(Assembly assembly, string name)
        {
            this.ReadText(Read.EmbeddedResource(assembly, name));
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public void ReadFile(string path)
        {
            this.ReadText(Read.File(path));
        }

        /// <summary>
        /// Reads the text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void ReadText(string text)
        {
            new PlainTextReader().ReadTo(text, this);
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public HeaderCollection Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Gets or sets the name of the feature.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for this feature.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the step definitions.
        /// </summary>
        /// <value>The step definitions.</value>
        public StepDefinitionCollection StepDefinitions
        {
            get { return this.stepDefinitions; }
        }

        /// <summary>
        /// Gets the scenarios.
        /// </summary>
        /// <value>The scenarios.</value>
        public ScenarioCollection Scenarios
        {
            get { return this.scenarios; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Feature"/> is passed.
        /// </summary>
        /// <value><c>true</c> if passed; otherwise, <c>false</c>.</value>
        public bool Passed
        {
            get { return this.passed; }
        }

        /// <summary>
        /// Gets or sets the reporter.
        /// </summary>
        /// <value>The reporter.</value>
        public Reporter Reporter
        {
            get { return this.reporter ?? (this.reporter = new DefaultReporter()); }
            set { this.reporter = value; }
        }

        /// <summary>
        /// Executes the scenarios.
        /// </summary>
        public void Execute()
        {
            this.passed = true;

            foreach (Scenario scenario in this.scenarios)
            {
                scenario.Execute();
                this.passed &= scenario.Passed;
            }
        }

        /// <summary>
        /// Reports this instance.
        /// </summary>
        public void Report()
        {
            this.Reporter.ReportFeature(this);
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        public void ReportUndefinedSteps()
        {
            this.Reporter.ReportUndefinedSteps(this.GetUndefinedSteps());
        }

        /// <summary>
        /// Gets the undefined steps.
        /// </summary>
        /// <returns>The union of undefined steps for all scenarios.</returns>
        public ICollection<Step> GetUndefinedSteps()
        {
            List<Step> undefinedSteps = new List<Step>();

            foreach (Scenario scenario in this.scenarios)
            {
                foreach (Step step in scenario.Steps)
                {
                    if (step.Result == StepResult.Undefined)
                    {
                        if (undefinedSteps.FindIndex(delegate(Step s) { return s.Text.Equals(step.Text, StringComparison.OrdinalIgnoreCase); }) == -1)
                        {
                            undefinedSteps.Add(step);
                        }
                    }
                }
            }

            return undefinedSteps;
        }

        /// <summary>
        /// Verifies this instance.
        /// </summary>
        public void Verify()
        {
            this.Execute();
            this.Report();

            var sw = new StringWriter();
            new PlainTextReporter(sw).ReportFeature(this);

            if (!this.Passed)
            {
                throw new VerificationException("\r\n\r\n" + sw.GetStringBuilder(), null);
            }
        }
    }
}
