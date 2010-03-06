using System.Diagnostics;

namespace BehaveN
{
    /// <summary>
    /// Represents an executable step in a scenario.
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class Step
    {
        /// <summary>
        /// The type of the step.
        /// </summary>
        public StepType Type { get; set; }

        /// <summary>
        /// The actual text for the step.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The block value or null for the step.
        /// </summary>
        public IBlock Block { get; set; }

        /// <summary>
        /// The result of executing the step.
        /// </summary>
        public StepResult Result { get; set; }
   }
}