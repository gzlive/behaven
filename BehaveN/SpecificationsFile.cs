// <copyright file="SpecificationsFile.cs" company="Jason Diamond">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Represents a specifications file.
    /// </summary>
    public class SpecificationsFile
    {
        private readonly HeaderCollection headers = new HeaderCollection();
        private readonly StepDefinitionCollection stepDefinitions = new StepDefinitionCollection();
        private readonly ScenarioCollection scenarios = new ScenarioCollection();
        private bool passed;
        private Reporter reporter;

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public HeaderCollection Headers
        {
            get { return this.headers; }
        }

        /// <summary>
        /// Gets or sets the title for these specifications.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description for these specifications.
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
        /// Gets a value indicating whether this <see cref="SpecificationsFile"/> is passed.
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
        /// Loads the file.
        /// </summary>
        /// <param name="path">The path to load from.</param>
        public void LoadFile(string path)
        {
            string text = File.ReadAllText(path);
            this.LoadText(text);
        }

        /// <summary>
        /// Loads the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="name">The name of the embedded resource.</param>
        public void LoadEmbeddedResource(Assembly assembly, string name)
        {
            string actualName = null;

            foreach (string possibleName in assembly.GetManifestResourceNames())
            {
                if (possibleName == name || possibleName.EndsWith("." + name, StringComparison.OrdinalIgnoreCase))
                {
                    actualName = possibleName;
                    break;
                }
            }

            if (actualName == null)
            {
                throw new Exception(string.Format("Could not find embedded resource named \"{0}\" in assembly named \"{1}\".", name, assembly.FullName));
            }

            using (Stream stream = assembly.GetManifestResourceStream(actualName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                this.LoadText(text);
            }
        }

        /// <summary>
        /// Loads the text.
        /// </summary>
        /// <param name="text">The text to read.</param>
        public void LoadText(string text)
        {
            PlainTextReader reader = new PlainTextReader();
            reader.ReadTo(text, this);

            foreach (Scenario scenario in this.scenarios)
            {
                scenario.StepDefinitions = this.stepDefinitions;
            }
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
            this.Reporter.ReportSpecificationsFile(this);
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

            if (!this.Passed)
            {
                throw new VerificationException(new Exception("Failed."));
            }
        }
    }
}
