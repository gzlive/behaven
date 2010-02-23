using System;

namespace BehaveN
{
    /// <summary>
    /// Identifies a step type.
    /// </summary>
    public enum StepType
    {
        /// <summary>
        /// Identifies an unspecified step.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Identifies a "name" step.
        /// </summary>
        Name,

        /// <summary>
        /// Identifies a "given" step.
        /// </summary>
        Given,

        /// <summary>
        /// Identifies a "when" step.
        /// </summary>
        When,

        /// <summary>
        /// Identifies a "then" step.
        /// </summary>
        Then
    }
}
