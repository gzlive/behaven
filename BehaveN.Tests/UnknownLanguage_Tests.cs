using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class UnknownLanguage_Tests : BaseScenarioTests
    {
        [Test]
        public void it_reads_unknown_languages_as_english()
        {
            ExecuteText("# language: xx",
                        "Scenario: Unknown language",
                        "Given foo",
                        "When bar",
                        "Then baz",
                        "And quux");

            TheScenario.Steps.Count.Should().Be(4);
            TheScenario.Passed.Should().Be.True();
        }

        public void given_foo() { }
        public void when_bar() { }
        public void then_baz() { }
        public void then_quux() { }
    }
}
