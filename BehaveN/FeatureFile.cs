using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BehaveN
{
    public class FeatureFile
    {
        private string _name;
        private readonly StepDefinitionCollection _stepDefinitions = new StepDefinitionCollection();
        private ScenarioCollection _scenarios = new ScenarioCollection();
        private bool _passed;
        private Reporter _reporter;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

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
        /// Gets a value indicating whether this <see cref="FeatureFile"/> is passed.
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
            get { return _reporter ?? (_reporter = new PlainTextReporter()); }
            set { _reporter = value; }
        }

        public void LoadFile(string path)
        {
            string text = File.ReadAllText(path);
            LoadText(text);
        }

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

        public void LoadText(string text)
        {
            PlainTextReader reader = new PlainTextReader(text);
            reader.ReadTo(this);

            foreach (Scenario scenario in _scenarios)
            {
                scenario.StepDefinitions = _stepDefinitions;
            }
        }

        public void Verify()
        {
            _passed = true;

            foreach (Scenario scenario in _scenarios)
            {
                scenario.Verify();
                _passed &= scenario.Passed;
            }
        }

        public void Report()
        {
            Reporter.ReportFeatureFile(this);
        }

        public void ReportUndefinedSteps()
        {
            Reporter.ReportUndefinedSteps(GetUndefinedSteps());
        }

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
    }
}
