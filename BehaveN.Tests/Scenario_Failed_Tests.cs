using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Failed_Tests : BaseScenarioTests
    {
        [Test]
        public void it_skips_all_steps_after_the_first_failed_step()
        {
            VerifyText("Scenario: Failed",
                       "Given some context",
                       "When a step fails",
                       "Then the remaining steps get skipped");

            TheScenario.Passed.Should().Be.False();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[1].Result.Should().Be(StepResult.Failed);
            TheScenario.Steps[2].Result.Should().Be(StepResult.Skipped);
        }

        public void given_some_context()
        {
        }

        public void when_a_step_fails()
        {
            throw new Exception("Failed!");
        }

        public void then_the_remaining_steps_get_skipped()
        {
        }
    }
}
