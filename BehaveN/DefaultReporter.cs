using System;
using System.Collections.Generic;
using System.IO;

namespace BehaveN
{
    /// <summary>
    /// Represents the default reporter.
    /// </summary>
    /// <remarks>The default reporter always reports to the console. It's
    /// <see cref="Reporter.Destination" /> property will create and use another
    /// reporter (based on the file extension of the new destination).</remarks>
    public class DefaultReporter : Reporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultReporter"/> class.
        /// </summary>
        public DefaultReporter()
        {
            _compositeReporter.Add(new PlainTextReporter(Console.Out));
        }

        private readonly CompositeReporter _compositeReporter = new CompositeReporter();

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

                if (_compositeReporter.Count > 1)
                {
                    _compositeReporter.RemoveAt(1);
                }

                _compositeReporter.Add(GetReporterBasedOnExtension(value, new StreamWriter(value)));
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
            _compositeReporter.ReportSpecificationsFile(specificationsFile);
        }

        /// <summary>
        /// Reports the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public override void ReportScenario(Scenario scenario)
        {
            _compositeReporter.ReportScenario(scenario);
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        /// <param name="undefinedSteps">The undefined steps.</param>
        public override void ReportUndefinedSteps(ICollection<Step> undefinedSteps)
        {
            _compositeReporter.ReportUndefinedSteps(undefinedSteps);
        }
    }
}
