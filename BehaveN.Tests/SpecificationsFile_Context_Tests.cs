using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class SpecificationsFile_Context_Tests : BaseSpecificationsFileTests
    {
        [Test]
        public void it_resets_the_context_between_each_scenario()
        {
            TheSpecificationsFile.StepDefinitions.UseStepDefinitionsFromType<MyGivenStepDefinitonsThatRequireMyContext>();
            TheSpecificationsFile.StepDefinitions.UseStepDefinitionsFromType<MyThenStepDefinitonsThatRequireMyContext>();

            LoadText("Scenario: Context",
                     "Given this foo",
                     "",
                     "Scenario: Context should be reset",
                     "Then the value should be foo");

            TheSpecificationsFile.Execute();

            TheSpecificationsFile.Passed.Should().Be.False();
        }
    }
}
