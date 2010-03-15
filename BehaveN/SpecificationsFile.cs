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

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BehaveN
{
    /// <summary>
    /// Represents a specifications file.
    /// </summary>
    public class SpecificationsFile
    {
        private readonly StepDefinitionCollection _stepDefinitions = new StepDefinitionCollection();
        private ScenarioCollection _scenarios = new ScenarioCollection();
        private bool _passed;
        private Reporter _reporter;

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
            get { return _stepDefinitions; }
        }

        /// <summary>
        /// Gets the scenarios.
        /// </summary>
        /// <value>The scenarios.</value>
        public ScenarioCollection Scenarios
        {
            get { return _scenarios; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SpecificationsFile"/> is passed.
        /// </summary>
        /// <value><c>true</c> if passed; otherwise, <c>false</c>.</value>
        public bool Passed
        {
            get { return _passed; }
        }

        /// <summary>
        /// Gets or sets the reporter.
        /// </summary>
        /// <value>The reporter.</value>
        public Reporter Reporter
        {
            get { return _reporter ?? (_reporter = new DefaultReporter()); }
            set { _reporter = value; }
        }

        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void LoadFile(string path)
        {
            string text = File.ReadAllText(path);
            LoadText(text);
        }

        /// <summary>
        /// Loads the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="name">The name.</param>
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
                LoadText(text);
            }
        }

        /// <summary>
        /// Loads the text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void LoadText(string text)
        {
            PlainTextReader reader = new PlainTextReader();
            reader.ReadTo(text, this);

            foreach (Scenario scenario in _scenarios)
            {
                scenario.StepDefinitions = _stepDefinitions;
            }
        }

        /// <summary>
        /// Executes the scenarios.
        /// </summary>
        public void Execute()
        {
            _passed = true;

            foreach (Scenario scenario in _scenarios)
            {
                scenario.Execute();
                _passed &= scenario.Passed;
            }
        }

        /// <summary>
        /// Reports this instance.
        /// </summary>
        public void Report()
        {
            Reporter.ReportSpecificationsFile(this);
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        public void ReportUndefinedSteps()
        {
            Reporter.ReportUndefinedSteps(GetUndefinedSteps());
        }

        /// <summary>
        /// Gets the undefined steps.
        /// </summary>
        /// <returns>The union of undefined steps for all scenarios.</returns>
        public ICollection<Step> GetUndefinedSteps()
        {
            List<Step> undefinedSteps = new List<Step>();

            foreach (Scenario scenario in _scenarios)
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
            Execute();
            Report();

            if (!Passed)
                throw new VerificationException(new Exception("Failed."));
        }
    }
}
