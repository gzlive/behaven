using System;
using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Undefined_Tests : BaseTests
    {
        [Test]
        public void it_marks_the_first_undefined_step_with_a_question_mark_and_skips_the_rest()
        {
            VerifyText("Given some defined context",
                       "When an undefined step is executed",
                       "Then other undefined steps get reported",
                       "And the remaining steps get skipped");

            ShouldHaveFailed();
            OutputShouldBe("  Given some defined context",
                           "",
                           "? When an undefined step is executed",
                           "",
                           "? Then other undefined steps get reported",
                           "- And the remaining steps get skipped",
                           "",
                           "Your undefined steps can be defined with the following code:",
                           "",
                           "public void when_an_undefined_step_is_executed()",
                           "{",
                           "    throw new NotImplementedException();",
                           "}",
                           "",
                           "public void then_other_undefined_steps_get_reported()",
                           "{",
                           "    throw new NotImplementedException();",
                           "}");
        }

        public void given_some_defined_context()
        {
        }

        public void then_the_remaining_steps_get_skipped()
        {
        }
    }
}
