using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter that can report to one or more child reporters.
    /// </summary>
    public class CompositeReporter : Reporter
    {
        List<Reporter> _children = new List<Reporter>();

        /// <summary>
        /// Reports the specifications file.
        /// </summary>
        /// <param name="specificationsFile">The specifications file.</param>
        /// <remarks>This reports all scenarios in the file and their
        /// undefined steps.</remarks>
        public override void ReportSpecificationsFile(SpecificationsFile specificationsFile)
        {
            foreach (var reporter in _children)
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
            foreach (var reporter in _children)
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
            foreach (var reporter in _children)
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
            _children.Add(reporter);
        }

        /// <summary>
        /// Removes the reporter at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            _children.RemoveAt(index);
        }

        /// <summary>
        /// Gets the count of reporters.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return _children.Count; } }
    }
}
