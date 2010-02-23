using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Outputs_Tests : BaseTests
    {
        [Test]
        public void it_passes_when_all_of_the_outputs_are_correct()
        {
            VerifyText("Then the string should be foo");

            ShouldHavePassed();
            OutputShouldBe("  Then the string should be foo");
        }

        [Test]
        public void it_fails_when_an_output_is_wrong()
        {
            VerifyText("Then the string should be string");

            ShouldHaveFailed();
            OutputShouldBe("! Then the string should be string (was foo)");
        }

        public void then_the_string_should_be_this(out string @this)
        {
            @this = "foo";
        }
    }
}
