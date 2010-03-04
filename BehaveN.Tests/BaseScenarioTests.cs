using NUnit.Framework;

namespace BehaveN.Tests
{
    public class BaseScenarioTests
    {
        [SetUp]
        public void BaseSetUp()
        {
            _specs = new SpecificationsFile();
            _specs.StepDefinitions.UseStepDefinitionsFromObject(this);
        }

        private SpecificationsFile _specs;
        private Scenario _scenario;

        protected SpecificationsFile TheSpecificationsFile { get { return _specs; } }
        protected Scenario TheScenario { get { return _scenario; } }

        protected void ExecuteText(params string[] lines)
        {
            string text = string.Join("\r\n", lines);
            _specs.LoadText(text);
            _scenario = _specs.Scenarios[0];
            _scenario.Execute();
        }
    }
}