using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Enum_Tests : BaseScenarioTests
    {
        [Test]
        public void it_passes_in_enums_correctly()
        {
            theEnum = TestEnum.Foo;

            ExecuteText("Scenario: Enum",
                        "Given the enum Bar");

            theEnum.Should().Be(TestEnum.Bar);
        }

        [Test]
        public void it_passes_in_nullable_enums_with_a_value_correctly()
        {
            theNullabelEnum = null;

            ExecuteText("Scenario: Nullable enum with a value",
                        "Given the nullable enum Bar");

            theNullabelEnum.Should().Be(TestEnum.Bar);
        }

        [Test]
        public void it_passes_in_nullable_enums_without_a_value_correctly()
        {
            theNullabelEnum = TestEnum.Bar;

            ExecuteText("Scenario: Nullable enum with a value",
                        "Given the nullable enum null");

            theNullabelEnum.Should().Be(null);
        }

        [Test]
        public void it_passes_when_output_enums_asserts_pass()
        {
            theEnum = TestEnum.Bar;

            ExecuteText("Scenario: Output int",
                        "Then the enum should be Bar");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_enums_asserts_fail()
        {
            theEnum = TestEnum.Bar;

            ExecuteText("Scenario: Output int",
                        "Then the enum should be Foo");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_enums_asserts_pass()
        {
            theNullabelEnum = TestEnum.Bar;

            ExecuteText("Scenario: Output nullable enum",
                        "Then the nullable enum should be Bar");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_enums_asserts_fail()
        {
            theNullabelEnum = TestEnum.Bar;

            ExecuteText("Scenario: Output nullable enum",
                        "Then the nullable enum should be Foo");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        [Test]
        public void it_passes_when_output_nullable_enums_expecting_null_asserts_pass()
        {
            theNullabelEnum = null;

            ExecuteText("Scenario: Output nullable enum",
                        "Then the nullable enum should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Passed);
        }

        [Test]
        public void it_fails_when_output_nullable_enums_expecting_null_asserts_fail()
        {
            theNullabelEnum = TestEnum.Foo;

            ExecuteText("Scenario: Output nullable enum",
                        "Then the nullable enum should be null");

            TheScenario.Steps[0].Result.Should().Be(StepResult.Failed);
        }

        private TestEnum theEnum;
        private TestEnum? theNullabelEnum;

        public void Given_the_enum_arg1(TestEnum arg1)
        {
            theEnum = arg1;
        }

        public void Given_the_nullable_enum_arg1(TestEnum? arg1)
        {
            theNullabelEnum = arg1;
        }

        public void Then_the_enum_should_be_arg1(out TestEnum arg1)
        {
            arg1 = theEnum;
        }

        public void Then_the_nullable_enum_should_be_arg1(out TestEnum? arg1)
        {
            arg1 = theNullabelEnum;
        }
    }
}
