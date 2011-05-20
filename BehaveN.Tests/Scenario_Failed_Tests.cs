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
            ExecuteText("Scenario: Failed",
                        "Given some context",
                        "When a step fails",
                        "Then the remaining steps get skipped");

            TheScenario.Passed.Should().Be.False();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[1].Result.Should().Be(StepResult.Failed);
            TheScenario.Steps[2].Result.Should().Be(StepResult.Skipped);
        }

        [Test]
        public void it_passes_null_into_the_next_step_if_the_next_steps_accepts_an_exception_argument_and_the_current_step_passed()
        {
            ExecuteText("Scenario: Failed",
                        "Given some context",
                        "When a step does not fail",
                        "Then the exception should be null");

            TheScenario.Passed.Should().Be.True();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[1].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[2].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_passes_the_exception_into_the_next_step_if_the_next_steps_accepts_an_exception_argument()
        {
            ExecuteText("Scenario: Failed",
                        "Given some context",
                        "When a step fails with message \"foo\"",
                        "Then the previous step should have failed with message \"foo\"");

            TheScenario.Passed.Should().Be.True();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[1].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[2].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_exceptions_are_thrown_by_the_exception_handler()
        {
            ExecuteText("Scenario: Failed",
                        "Given some context",
                        "When a step fails with message \"foo\"",
                        "Then the previous step should have failed with message \"bar\"");

            TheScenario.Passed.Should().Be.False();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[1].Result.Should().Be(StepResult.Passed);
            TheScenario.Steps[2].Result.Should().Be(StepResult.Failed);
            TheScenario.Steps[2].Text.Should().Be("Then the previous step should have failed with message \"bar (was foo)\"");
        }

        public void given_some_context()
        {
        }

        public void when_a_step_fails()
        {
            throw new Exception("Failed!");
        }

        public void when_a_step_does_not_fail()
        {
        }

        public void when_a_step_fails_with_message_arg1(string message)
        {
            throw new Exception(message);
        }

        public void then_the_remaining_steps_get_skipped()
        {
        }

        public void then_the_exception_should_be_null(Exception e)
        {
            e.Should().Be.Null();
        }

        public void then_the_previous_step_should_have_failed_with_message_arg1(out string message, Exception e)
        {
            message = e.Message;
        }
    }
}
