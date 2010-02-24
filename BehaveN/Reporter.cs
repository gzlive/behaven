using System;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter for executed steps.
    /// </summary>
    public abstract class Reporter
    {
        public abstract void Begin();
        public abstract void ReportScenario(Scenario scenario);
        public abstract void ReportUndefinedSteps(List<Step> undefinedSteps);
        public abstract void End();
    }
}
