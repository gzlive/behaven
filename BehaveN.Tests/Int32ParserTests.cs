using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Describe_Int32Parser
    {
        [Test]
        public void it_should_parse_1_correctly()
        {
            int i = Int32Parser.ParseInt32("1");

            i.Should().Be(1);
        }

        [Test]
        public void it_should_parse_negative_1_correctly()
        {
            int i = Int32Parser.ParseInt32("-1");

            i.Should().Be(-1);
        }

        [Test]
        public void it_should_parse_1st_correctly()
        {
            int i = Int32Parser.ParseInt32("1st");

            i.Should().Be(1);
        }

        [Test]
        public void it_should_parse_2nd_correctly()
        {
            int i = Int32Parser.ParseInt32("2nd");

            i.Should().Be(2);
        }

        [Test]
        public void it_should_parse_3rd_correctly()
        {
            int i = Int32Parser.ParseInt32("3rd");

            i.Should().Be(3);
        }

        [Test]
        public void it_should_parse_4th_correctly()
        {
            int i = Int32Parser.ParseInt32("4th");

            i.Should().Be(4);
        }

        [Test]
        public void it_should_return_0_when_parsing_fails()
        {
            int i = Int32Parser.ParseInt32("xxx");

            i.Should().Be(0);
        }

        [Test]
        public void it_should_return_the_specified_default_when_parsing_fails()
        {
            int i = Int32Parser.ParseInt32("xxx", int.MinValue);

            i.Should().Be(int.MinValue);
        }
    }
}
