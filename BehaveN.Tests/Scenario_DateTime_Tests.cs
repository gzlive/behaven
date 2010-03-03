using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_DateTime_Tests : BaseScenarioTests
    {
        [Test]
        public void it_passes_in_date_times_correctly()
        {
            theDateTime = DateTime.MinValue;

            ExecuteText("Scenario: DateTime",
                        "Given the date time 2010-02-28");

            theDateTime.Should().Be(new DateTime(2010, 2, 28));
        }

        [Test]
        public void it_passes_in_nullable_date_times_with_a_value_correctly()
        {
            theNullabelDateTime = new DateTime(2010, 2, 28);

            ExecuteText("Scenario: Nullable date time with a value",
                        "Given the nullable date time 2010-02-28");

            theNullabelDateTime.Should().Be(new DateTime(2010, 2, 28));
        }

        [Test]
        public void it_passes_in_nullable_date_times_without_a_value_correctly()
        {
            theNullabelDateTime = null;

            ExecuteText("Scenario: Nullable date time without a value",
                        "Given the nullable date time null");

            theNullabelDateTime.Should().Be(null);
        }

        [Test]
        public void it_passes_when_output_date_times_asserts_pass()
        {
            theDateTime = new DateTime(2010, 2, 28);

            ExecuteText("Scenario: Output date time passes",
                        "Then the date time should be 2010-02-28");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_date_times_asserts_fail()
        {
            theDateTime = new DateTime(2010, 3, 1);

            ExecuteText("Scenario: Output date time fails",
                        "Then the date time should be 2010-02-28");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_date_times_asserts_pass()
        {
            theNullabelDateTime = new DateTime(2010, 2, 28);

            ExecuteText("Scenario: Output nullable date time passes",
                        "Then the nullable date time should be 2010-02-28");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_date_times_asserts_fail()
        {
            theNullabelDateTime = new DateTime(2010, 3, 1);

            ExecuteText("Scenario: Output nullable date time fails",
                        "Then the nullable date time should be 2010-02-28");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_date_times_expecting_null_asserts_pass()
        {
            theNullabelDateTime = null;

            ExecuteText("Scenario: Output nullable date time passes",
                        "Then the nullable date time should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_date_times_expecting_null_asserts_fail()
        {
            theNullabelDateTime = new DateTime(2010, 2, 28);

            ExecuteText("Scenario: Output nullable date time fails",
                        "Then the nullable date time should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        private DateTime theDateTime;
        private DateTime? theNullabelDateTime;

        public void Given_the_date_time_arg1(DateTime arg1)
        {
            theDateTime = arg1;
        }

        public void Given_the_nullable_date_time_arg1(DateTime? arg1)
        {
            theNullabelDateTime = arg1;
        }

        public void Then_the_date_time_should_be_arg1(out DateTime arg1)
        {
            arg1 = theDateTime;
        }

        public void Then_the_nullable_date_time_should_be_arg1(out DateTime? arg1)
        {
            arg1 = theNullabelDateTime;
        }
    }
}
