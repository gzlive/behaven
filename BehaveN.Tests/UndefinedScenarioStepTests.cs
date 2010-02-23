using System.IO;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class UndefinedScenarioStepTests
    {
        [Test]
        public void Test()
        {
            Scenario scenario = new Scenario();
            StringWriter sw = new StringWriter();
            scenario.Reporter = new PlainTextScenarioReporter(sw);

            Assert.Throws(typeof(VerificationException), () => scenario.VerifyText(
                  "Given foo\r\n"
                + "And bar baz\r\n"
                + "And 123 as an integer and 4.56 as a decimal and \"abc xyz\" as a string\r\n"
                + "And $123 as another decimal\r\n"
                + "And this grid\r\n"
                + "  |foo|bar|baz|\r\n"
                + "  | a1| a2| a3|\r\n"
            ));

            sw.GetStringBuilder().ToString().Should().Be(
                  "? Given foo\r\n"
                + "? And bar baz\r\n"
                + "? And 123 as an integer and 4.56 as a decimal and \"abc xyz\" as a string\r\n"
                + "? And $123 as another decimal\r\n"
                + "? And this grid\r\n"
                + "    | foo | bar | baz |\r\n"
                + "    |  a1 |  a2 |  a3 |\r\n"
                + "\r\n"
                + "Your undefined steps can be defined with the following code:\r\n"
                + "\r\n"
                + "public void given_foo()\r\n"
                + "{\r\n"
                + "    throw new NotImplementedException();\r\n"
                + "}\r\n"
                + "\r\n"
                + "public void given_bar_baz()\r\n"
                + "{\r\n"
                + "    throw new NotImplementedException();\r\n"
                + "}\r\n"
                + "\r\n"
                + "public void given_x1_as_an_integer_and_x2_as_a_decimal_and_x3_as_a_string(int x1, decimal x2, string x3)\r\n"
                + "{\r\n"
                + "    throw new NotImplementedException();\r\n"
                + "}\r\n"
                + "\r\n"
                + "public void given_x1_as_another_decimal(decimal x1)\r\n"
                + "{\r\n"
                + "    throw new NotImplementedException();\r\n"
                + "}\r\n"
                + "\r\n"
                + "public void given_this_grid(Grid grid)\r\n"
                + "{\r\n"
                + "    throw new NotImplementedException();\r\n"
                + "}\r\n"
                + "\r\n"
            );
        }
    }
}
