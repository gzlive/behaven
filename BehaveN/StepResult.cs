namespace BehaveN
{
    /// <summary>
    /// Represents the result of executing a step.
    /// </summary>
    public enum StepResult
    {
        /// <summary>
        /// The step hasn't been executed yet.
        /// </summary>
        Unknown,

        /// <summary>
        /// The step passed.
        /// </summary>
        Passed,

        /// <summary>
        /// The step failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The step was not defined.
        /// </summary>
        Undefined,

        /// <summary>
        /// The step threw NotImplementedException.
        /// </summary>
        Pending,

        /// <summary>
        /// The step was skipped.
        /// </summary>
        Skipped
    }
}
