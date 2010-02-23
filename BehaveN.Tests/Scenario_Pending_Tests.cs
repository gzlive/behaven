using System;
using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Pending_Tests : BaseTests
    {
        [Test]
        public void it_marks_the_first_pending_step_with_a_star_and_skips_the_rest()
        {
            VerifyText("Given some non pending context",
                       "When a pending step is executed",
                       "Then the remaining steps get skipped");

            ShouldHaveFailed();
            OutputShouldBe("  Given some non pending context",
                           "",
                           "* When a pending step is executed",
                           "",
                           "- Then the remaining steps get skipped");
        }

        public void given_some_non_pending_context()
        {
        }

        public void when_a_pending_step_is_executed()
        {
            throw new NotImplementedException();
        }

        public void then_the_remaining_steps_get_skipped()
        {
        }
    }
}
