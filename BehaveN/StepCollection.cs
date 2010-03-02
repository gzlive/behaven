using System;
using System.Collections;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents an executable step in a scenario.
    /// </summary>
    public class StepCollection : IEnumerable<Step>
    {
        List<Step> _steps = new List<Step>();

        /// <summary>
        /// Adds a step with the specified information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="step">The step.</param>
        /// <param name="block">The block.</param>
        public void Add(StepType type, string step, IBlock block)
        {
            Step newStep = new Step();
            newStep.Type = type;
            newStep.Text = step;
            newStep.Block = block;
            Add(newStep);
        }

        /// <summary>
        /// Adds the specified step.
        /// </summary>
        /// <param name="step">The step.</param>
        public void Add(Step step)
        {
            _steps.Add(step);
        }

        /// <summary>
        /// Gets the <see cref="BehaveN.Step"/> at the specified index.
        /// </summary>
        /// <value>The step.</value>
        public Step this[int index]
        {
            get
            {
                return _steps[index];
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return _steps.Count; } }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Step> GetEnumerator()
        {
            return _steps.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}