using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Int32_Tests : BaseScenarioTests
    {
        [Test]
        public void it_passes_in_ints_correctly()
        {
            theInt = 0;

            VerifyText("Scenario: Int",
                       "Given the int 123");

            theInt.Should().Be(123);
        }

        [Test]
        public void it_passes_in_nullable_ints_with_a_value_correctly()
        {
            theNullabelInt = null;

            VerifyText("Scenario: Nullable int with a value",
                       "Given the nullable int 123");

            theNullabelInt.Should().Be(123);
        }

        [Test]
        public void it_passes_in_nullable_ints_without_a_value_correctly()
        {
            theNullabelInt = 123;

            VerifyText("Scenario: Nullable int with a value",
                       "Given the nullable int null");

            theNullabelInt.Should().Be(null);
        }

        [Test]
        public void it_passes_when_output_ints_asserts_pass()
        {
            theInt = 123;

            VerifyText("Scenario: Output int",
                       "Then the int should be 123");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_ints_asserts_fail()
        {
            theInt = 456;

            VerifyText("Scenario: Output int",
                       "Then the int should be 123");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_ints_asserts_pass()
        {
            theNullabelInt = 123;

            VerifyText("Scenario: Output nullable int",
                       "Then the nullable int should be 123");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_ints_asserts_fail()
        {
            theNullabelInt = 456;

            VerifyText("Scenario: Output nullable int",
                       "Then the nullable int should be 123");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_ints_expecting_null_asserts_pass()
        {
            theNullabelInt = null;

            VerifyText("Scenario: Output nullable int",
                       "Then the nullable int should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_ints_expecting_null_asserts_fail()
        {
            theNullabelInt = 123;

            VerifyText("Scenario: Output nullable int",
                       "Then the nullable int should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        private int theInt;
        private int? theNullabelInt;

        public void Given_the_int_arg1(int arg1)
        {
            theInt = arg1;
        }

        public void Given_the_nullable_int_arg1(int? arg1)
        {
            theNullabelInt = arg1;
        }

        public void Then_the_int_should_be_arg1(out int arg1)
        {
            arg1 = theInt;
        }

        public void Then_the_nullable_int_should_be_arg1(out int? arg1)
        {
            arg1 = theNullabelInt;
        }
    }
}
