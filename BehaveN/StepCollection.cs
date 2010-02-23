using System;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a collection of steps.
    /// </summary>
    public class StepCollection
    {
        private List<Step> _steps = new List<Step>();

        /// <summary>
        /// Adds a step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="description">The description.</param>
        public void Add(StepType stepType, string description)
        {
            _steps.Add(new DescriptionStep(stepType, description));
        }

        /// <summary>
        /// Adds a step.
        /// </summary>
        /// <param name="stepType">Type of the step.</param>
        /// <param name="text">The text.</param>
        /// <param name="convertibleObject">The convertible object.</param>
        public void Add(StepType stepType, string text, IConvertibleObject convertibleObject)
        {
            _steps.Add(new TextStep(stepType, text, convertibleObject));
        }

        /// <summary>
        /// Clears the steps.
        /// </summary>
        public void Clear()
        {
            _steps.Clear();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _steps.Count; }
        }

        /// <summary>
        /// Gets the <see cref="BehaveN.Step"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Step this[int index]
        {
            get { return _steps[index];  }
        }
    }
}
