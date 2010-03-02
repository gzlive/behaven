using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class FeatureFile_Context_Tests : BaseFeatureFileTests
    {
        [Test]
        public void it_resets_the_context_between_each_scenario()
        {
            TheFeatureFile.StepDefinitions.UseStepDefinitionsFromType<MyGivenStepDefinitonsThatRequireMyContext>();
            TheFeatureFile.StepDefinitions.UseStepDefinitionsFromType<MyThenStepDefinitonsThatRequireMyContext>();

            LoadText("Scenario: Context",
                     "Given this foo",
                     "",
                     "Scenario: Context should be reset",
                     "Then the value should be foo");

            TheFeatureFile.Execute();

            TheFeatureFile.Passed.Should().Be.False();
        }
    }
}
