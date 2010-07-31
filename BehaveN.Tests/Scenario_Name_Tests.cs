using System.Reflection;
using System.Text;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Name_Tests : BaseScenarioTests
    {
        [Test]
        public void it_does_not_throw_when_the_first_scenario_does_not_have_a_name()
        {
            ExecuteText("Given a foo");

            TheScenario.Name.Should().Be("(no name)");
        }

        [Test]
        public void it_does_not_throw_when_the_scenario_line_does_not_have_a_name()
        {
            ExecuteText("Scenario:",
                        "Given a foo");

            TheScenario.Name.Should().Be("(no name)");
        }
    }
}
