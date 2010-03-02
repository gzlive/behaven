namespace BehaveN
{
    /// <summary>
    /// Represents an executable step in a scenario.
    /// </summary>
    public class Step
    {
        /// <summary>
        /// The type of the step.
        /// </summary>
        public StepType Type;

        /// <summary>
        /// The actual text for the step.
        /// </summary>
        public string Text;

        /// <summary>
        /// The block value or null for the step.
        /// </summary>
        public IBlock Block;

        /// <summary>
        /// The result of executing the step.
        /// </summary>
        public StepResult Result;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Text;
        }
    }
}