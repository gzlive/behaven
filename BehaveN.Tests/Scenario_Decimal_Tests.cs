using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Decimal_Tests : BaseScenarioTests
    {
        [Test]
        public void it_passes_in_ints_correctly()
        {
            theDecimal = 0;

            ExecuteText("Scenario: Decimal",
                        "Given the decimal 1.23");

            theDecimal.Should().Be(1.23m);
        }

        [Test]
        public void it_passes_in_nullable_decimals_with_a_value_correctly()
        {
            theNullabelDecimal = null;

            ExecuteText("Scenario: Nullable decimal with a value",
                        "Given the nullable decimal 1.23");

            theNullabelDecimal.Should().Be(1.23m);
        }

        [Test]
        public void it_passes_in_nullable_decimals_without_a_value_correctly()
        {
            theNullabelDecimal = 1.23m;

            ExecuteText("Scenario: Nullable decimal with a value",
                        "Given the nullable decimal null");

            theNullabelDecimal.Should().Be(null);
        }

        [Test]
        public void it_passes_when_output_decimals_asserts_pass()
        {
            theDecimal = 1.23m;

            ExecuteText("Scenario: Output decimal ",
                        "Then the decimal should be 1.23");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_decimals_asserts_fail()
        {
            theDecimal = 4.56m;

            ExecuteText("Scenario: Output decimal",
                        "Then the decimal should be 1.23");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_decimals_asserts_pass()
        {
            theNullabelDecimal = 1.23m;

            ExecuteText("Scenario: Output nullable decimal",
                        "Then the nullable decimal should be 1.23");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_decimals_asserts_fail()
        {
            theNullabelDecimal = 4.56m;

            ExecuteText("Scenario: Output nullable decimal",
                        "Then the nullable decimal should be 1.23");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_decimals_expecting_null_asserts_pass()
        {
            theNullabelDecimal = null;

            ExecuteText("Scenario: Output nullable decimal",
                        "Then the nullable decimal should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_decimals_expecting_null_asserts_fail()
        {
            theNullabelDecimal = 123;

            ExecuteText("Scenario: Output nullable decimal",
                        "Then the nullable decimal should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        private decimal theDecimal;
        private decimal? theNullabelDecimal;

        public void Given_the_decimal_arg1(decimal arg1)
        {
            theDecimal = arg1;
        }

        public void Given_the_nullable_decimal_arg1(decimal? arg1)
        {
            theNullabelDecimal = arg1;
        }

        public void Then_the_decimal_should_be_arg1(out decimal arg1)
        {
            arg1 = theDecimal;
        }

        public void Then_the_nullable_decimal_should_be_arg1(out decimal? arg1)
        {
            arg1 = theNullabelDecimal;
        }
    }
}
