using System.Text;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Text_Tests : BaseScenarioTests
    {
        private StringBuilder _stringBuilder;

        [SetUp]
        public void SetUp()
        {
            _stringBuilder = null;
        }

        [Test]
        public void it_converts_text_blocks_into_a_string_builder()
        {
            ExecuteText("Scenario: Text",
                        "Given this text",
                        "  > This is line 1",
                        "  > This is line 2");

            _stringBuilder.Should().Not.Be.Null();
            _stringBuilder.ToString().Should().Be("This is line 1\r\nThis is line 2");
        }

        public void given_this_text(StringBuilder sb)
        {
            _stringBuilder = sb;
        }
    }
}
