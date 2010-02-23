using System;

namespace BehaveN
{
    /// <summary>
    /// Represents a reporter for executed steps.
    /// </summary>
    public abstract class Reporter
    {
        /// <summary>
        /// Reports the description of the scenario or concern.
        /// </summary>
        /// <param name="description">The description.</param>
        public abstract void ReportDescription(string description);

        /// <summary>
        /// Reports an undefined step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        public abstract void ReportUndefined(StepType stepType, string description, IConvertibleObject convertibleObject);

        /// <summary>
        /// Reports a pending step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        public abstract void ReportPending(StepType stepType, string description);

        /// <summary>
        /// Reports a passed step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        public abstract void ReportPassed(StepType stepType, string description);

        /// <summary>
        /// Reports a failed step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        /// <param name="e">The exception thrown by the step.</param>
        public abstract void ReportFailed(StepType stepType, string description, Exception e);

        /// <summary>
        /// Reports a skipped step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        public abstract void ReportSkipped(StepType stepType, string description);

        /// <summary>
        /// Reports a convertible object that was associated with a step.
        /// </summary>
        /// <param name="convertibleObject">The convertible object.</param>
        public abstract void ReportConvertibleObject(IConvertibleObject convertibleObject);

        /// <summary>
        /// Reports the end.
        /// </summary>
        public abstract void ReportEnd();
    }
}
