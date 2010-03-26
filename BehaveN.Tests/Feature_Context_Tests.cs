using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Feature_Context_Tests : BaseFeatureTests
    {
        [Test]
        public void it_resets_the_context_between_each_scenario()
        {
            TheFeature.StepDefinitions.UseStepDefinitionsFromType<MyGivenStepDefinitonsThatRequireMyContext>();
            TheFeature.StepDefinitions.UseStepDefinitionsFromType<MyThenStepDefinitonsThatRequireMyContext>();

            LoadText("Scenario: Context",
                     "Given this foo",
                     "",
                     "Scenario: Context should be reset",
                     "Then the value should be foo");

            TheFeature.Execute();

            TheFeature.Passed.Should().Be.False();
        }
    }
}
