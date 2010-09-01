using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Step_Tests : BaseScenarioTests
    {
        private bool _given_a_string;
        private bool _given_a_string_and_another_string;

        [Test]
        public void it_does_not_get_confused_by_two_step_definitions_that_start_the_same()
        {
            _given_a_string = false;
            _given_a_string_and_another_string = false;

            ExecuteText("Given a string \"foo\" and another string \"bar\"");

            _given_a_string.Should().Be(false);
            _given_a_string_and_another_string.Should().Be(true);
        }

        public void given_a_string_arg1(string s)
        {
            _given_a_string = true;
        }

        public void given_a_string_arg1_and_another_string_arg2(string s1, string s2)
        {
            _given_a_string_and_another_string = true;
        }
    }
}
