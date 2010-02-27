using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Outputs_Tests : BaseScenarioTests
    {
        [Test]
        public void it_passes_when_all_of_the_outputs_are_correct()
        {
            VerifyText("Scenario: Output passed",
                       "Then the string should be foo");

            TheScenario.Passed.Should().Be.True();
        }

        [Test]
        public void it_fails_when_an_output_is_wrong()
        {
            VerifyText("Scenario: Output failed",
                       "Then the string should be string");

            TheScenario.Passed.Should().Be.False();
            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
            TheScenario.Steps[0].Text.Should().Be("Then the string should be string (was foo)");
        }

        public void then_the_string_should_be_arg1(out string value)
        {
            value = "foo";
        }
    }
}
