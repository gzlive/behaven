using NUnit.Framework;

namespace BehaveN.Tests
{
    public class BaseScenarioTests
    {
        [SetUp]
        public void BaseSetUp()
        {
            _ff = new FeatureFile();
            _ff.StepDefinitions.UseStepDefinitionsFromObject(this);
        }

        private FeatureFile _ff;
        private Scenario _s;

        protected FeatureFile TheFeatureFile { get { return _ff; } }
        protected Scenario TheScenario { get { return _s; } }

        protected void VerifyText(params string[] lines)
        {
            string text = string.Join("\r\n", lines);
            _ff.LoadText(text);
            _s = _ff.Scenarios[0];
            _s.Verify();
        }
    }
}