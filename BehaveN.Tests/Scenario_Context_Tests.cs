using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Context_Tests : BaseScenarioTests
    {
        [Test]
        public void the_same_context_object_is_passed_in_to_all_step_definition_classes()
        {
            TheFeature.StepDefinitions.UseStepDefinitionsFromType<MyGivenStepDefinitonsThatRequireMyContext>();
            TheFeature.StepDefinitions.UseStepDefinitionsFromType<MyThenStepDefinitonsThatRequireMyContext>();

            ExecuteText("Scenario: Context",
                        "Given this foo",
                        "Then the value should be foo");

            TheScenario.Passed.Should().Be.True();
        }

        [Test]
        public void context_objects_that_implement_IDisposable_are_disposed_after_the_scenario_is_executed()
        {
            MyContext.DisposeWasInvoked = false;

            TheFeature.StepDefinitions.UseStepDefinitionsFromType<MyGivenStepDefinitonsThatRequireMyContext>();

            ExecuteText("Scenario: Dispose",
                        "Given this foo");

            MyContext.DisposeWasInvoked.Should().Be(true);
        }
    }

    public class MyGivenStepDefinitonsThatRequireMyContext
    {
        private readonly MyContext myContext;

        public MyGivenStepDefinitonsThatRequireMyContext(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public void Given_this_arg1(string value)
        {
            this.myContext.MyString = value;
        }
    }

    public class MyThenStepDefinitonsThatRequireMyContext
    {
        private readonly MyContext myContext;

        public MyThenStepDefinitonsThatRequireMyContext(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public void Then_the_value_should_be_arg1(string value)
        {
            this.myContext.MyString.Should().Be(value);
        }
    }

    public class MyContext : IDisposable
    {
        public static bool DisposeWasInvoked;

        public string MyString { get; set; }

        public void Dispose()
        {
            DisposeWasInvoked = true;
        }
    }
}
