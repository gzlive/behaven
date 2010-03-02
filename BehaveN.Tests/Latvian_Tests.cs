using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Latvian_Tests : BaseScenarioTests
    {
        [Test]
        public void it_knows_how_to_read_latvian()
        {
            ExecuteText("# language: lv",
                        "Scenārijs: latviešu",
                        "Kad foo",
                        "Ja bar",
                        "Tad baz",
                        "Un quux");

            TheScenario.Steps.Count.Should().Be(4);
            TheScenario.Passed.Should().Be.True();
        }

        public void given_foo() { }
        public void when_bar() { }
        public void then_baz() { }
        public void then_quux() { }
    }
}
