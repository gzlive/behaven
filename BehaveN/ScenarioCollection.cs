// <copyright file="ScenarioCollection.cs" company="Jason Diamond">
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
    /// Represents a collection of scenarios.
    /// </summary>
    public class ScenarioCollection : IEnumerable<Scenario>
    {
        private readonly List<Scenario> scenarios = new List<Scenario>();

        /// <summary>
        /// Gets the count of scenarios.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return this.scenarios.Count; }
        }

        /// <summary>
        /// Gets the <see cref="BehaveN.Scenario"/> at the specified index.
        /// </summary>
        /// <param name="index">The requested index.</param>
        public Scenario this[int index]
        {
            get { return this.scenarios[index]; }
        }

        /// <summary>
        /// Gets the <see cref="BehaveN.Scenario"/> with the specified name.
        /// </summary>
        /// <param name="name">The requested scenario name.</param>
        public Scenario this[string name]
        {
            get { return this.scenarios.Find(delegate(Scenario s) { return s.Name == name; }); }
        }

        /// <summary>
        /// Adds the specified scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public void Add(Scenario scenario)
        {
            this.scenarios.Add(scenario);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Scenario> GetEnumerator()
        {
            return this.scenarios.GetEnumerator();
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