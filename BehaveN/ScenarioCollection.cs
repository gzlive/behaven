using System.Collections;
using System.Collections.Generic;

namespace BehaveN
{
    public class ScenarioCollection : IEnumerable<Scenario>
    {
        private readonly List<Scenario> _scenarios = new List<Scenario>();

        public void Add(Scenario scenario)
        {
            _scenarios.Add(scenario);
        }

        public int Count { get { return _scenarios.Count; } }

        public Scenario this[int index]
        {
            get { return _scenarios[index]; }
        }

        public Scenario this[string name]
        {
            get { return _scenarios.Find(delegate(Scenario s) { return s.Name == name; }); }
        }

        public IEnumerator<Scenario> GetEnumerator()
        {
            return _scenarios.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}