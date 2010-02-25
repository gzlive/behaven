using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter for executed steps.
    /// </summary>
    public abstract class Reporter
    {
        public abstract void ReportFeatureFile(FeatureFile featureFile);
        public abstract void ReportScenario(Scenario scenario);
        public abstract void ReportUndefinedSteps(ICollection<Step> undefinedSteps);
    }
}
