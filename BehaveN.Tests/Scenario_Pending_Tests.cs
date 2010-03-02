using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Pending_Tests : BaseScenarioTests
    {
        [Test]
        public void it_skips_all_steps_after_the_first_pending_step()
        {
            ExecuteText("Scenario: Pending",
                        "Given some context",
                        "When a pending step is executed",
                        "Then the remaining steps get skipped");

            TheScenario.Passed.Should().Be.False();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[1].Result.Should().Be(StepResult.Pending);
            TheScenario.Steps[2].Result.Should().Be(StepResult.Skipped);
        }

        public void given_some_context()
        {
        }

        public void when_a_pending_step_is_executed()
        {
            throw new NotImplementedException();
        }

        public void then_the_remaining_steps_get_skipped()
        {
            throw new NotImplementedException();
        }
    }
}
