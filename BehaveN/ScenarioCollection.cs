using System.Collections;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a collection of scenarios.
    /// </summary>
    public class ScenarioCollection : IEnumerable<Scenario>
    {
        private readonly List<Scenario> _scenarios = new List<Scenario>();

        /// <summary>
        /// Adds the specified scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public void Add(Scenario scenario)
        {
            _scenarios.Add(scenario);
        }

        /// <summary>
        /// Gets the count of scenarios.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return _scenarios.Count; } }

        /// <summary>
        /// Gets the <see cref="BehaveN.Scenario"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Scenario this[int index]
        {
            get { return _scenarios[index]; }
        }

        /// <summary>
        /// Gets the <see cref="BehaveN.Scenario"/> with the specified name.
        /// </summary>
        /// <value></value>
        public Scenario this[string name]
        {
            get { return _scenarios.Find(delegate(Scenario s) { return s.Name == name; }); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Scenario> GetEnumerator()
        {
            return _scenarios.GetEnumerator();
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