using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Undefined_Tests : BaseScenarioTests
    {
        [Test]
        public void it_skips_all_defined_steps_after_the_first_undefined_steps()
        {
            ExecuteText("Scenario: Undefined",
                        "Given some context",
                        "When an undefined step is executed",
                        "Then other undefined steps get reported",
                        "And the remaining steps get skipped");

            TheScenario.Passed.Should().Be.False();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[1].Result.Should().Be(StepResult.Undefined);
            TheScenario.Steps[2].Result.Should().Be(StepResult.Undefined);
            TheScenario.Steps[3].Result.Should().Be(StepResult.Skipped);
        }

        public void given_some_context()
        {
        }

        public void then_the_remaining_steps_get_skipped()
        {
        }
    }
}
