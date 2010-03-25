// <copyright file="StepCollection.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

namespace BehaveN
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an executable step in a scenario.
    /// </summary>
    public class StepCollection : IEnumerable<Step>
    {
        private readonly List<Step> steps = new List<Step>();

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return this.steps.Count; }
        }

        /// <summary>
        /// Gets the <see cref="BehaveN.Step"/> at the specified index.
        /// </summary>
        /// <param name="index">The requested index.</param>
        public Step this[int index]
        {
            get
            {
                return this.steps[index];
            }
        }

        /// <summary>
        /// Adds a step with the specified information.
        /// </summary>
        /// <param name="type">The step type.</param>
        /// <param name="step">The step text.</param>
        /// <param name="block">The block value.</param>
        public void Add(StepType type, string step, IBlock block)
        {
            Step newStep = new Step();
            newStep.Type = type;
            newStep.Text = step;
            newStep.Block = block;
            this.Add(newStep);
        }

        /// <summary>
        /// Adds the specified step.
        /// </summary>
        /// <param name="step">The step to add.</param>
        public void Add(Step step)
        {
            this.steps.Add(step);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Step> GetEnumerator()
        {
            return this.steps.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}