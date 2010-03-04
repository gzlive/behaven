using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter for executed steps.
    /// </summary>
    public abstract class Reporter
    {
        /// <summary>
        /// Reports the specifications file.
        /// </summary>
        /// <param name="specificationsFile">The specifications file.</param>
        public abstract void ReportSpecificationsFile(SpecificationsFile specificationsFile);

        /// <summary>
        /// Reports the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public abstract void ReportScenario(Scenario scenario);

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        /// <param name="undefinedSteps">The undefined steps.</param>
        public abstract void ReportUndefinedSteps(ICollection<Step> undefinedSteps);
    }
}
