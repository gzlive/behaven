using NUnit.Framework;

namespace BehaveN.Tests
{
    public class BaseScenarioTests
    {
        [SetUp]
        public void BaseSetUp()
        {
            _feature = new Feature();
            _feature.StepDefinitions.UseStepDefinitionsFromObject(this);
        }

        private Feature _feature;
        private Scenario _scenario;

        protected Feature TheFeature { get { return _feature; } }
        protected Scenario TheScenario { get { return _scenario; } }

        protected void ExecuteText(params string[] lines)
        {
            string text = string.Join("\r\n", lines);
            _feature.LoadText(text);
            _scenario = _feature.Scenarios[0];
            _scenario.Execute();
        }
    }
}