using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class StepAttribute_Tests : BaseScenarioTests
    {
        [Test]
        public void it_uses_the_method_name_even_if_when_it_has_a_step_attribute()
        {
            ExecuteText("Scenario: Method name",
                        "Given foo");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_uses_the_text_in_the_first_step_attribute()
        {
            ExecuteText("Scenario: First attribute",
                        "Given bar");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_uses_the_text_in_the_second_step_attribute()
        {
            ExecuteText("Scenario: Second attribute",
                        "Given baz");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_pass_in_args()
        {
            this.theInt = 0;

            ExecuteText("Scenario: Argument",
                        "Given the number 123");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
            this.theInt.Should().Be(123);
        }

        [Step("given bar")]
        [Step("given baz")]
        public void given_foo()
        {
        }

        private int theInt;

        [Step("given the number arg1")]
        public void given_the_int_arg1(int n)
        {
            this.theInt = n;
        }
    }
}
