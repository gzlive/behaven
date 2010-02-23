using System.IO;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    public class BaseTests
    {
        [SetUp]
        public void BaseSetUp()
        {
            s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            sw = new StringWriter();
            s.Reporter = new PlainTextScenarioReporter(sw);
        }

        protected Scenario s;
        private bool passed;
        private StringWriter sw;

        protected void VerifyText(params string[] textLines)
        {
            string text = string.Join("\r\n", textLines);

            try
            {
                s.VerifyText(text);
                passed = true;
            }
            catch (VerificationException)
            {
                passed = false;
            }
        }

        protected void ShouldHavePassed()
        {
            passed.Should().Be.True();
        }

        protected void ShouldHaveFailed()
        {
            passed.Should().Be.False();
        }

        protected void OutputShouldBe(params string[] outputLines)
        {
            string actual = sw.GetStringBuilder().ToString();
            string expected = string.Join("\r\n", outputLines) + "\r\n\r\n";

            actual.Should().Be(expected);
        }
    }
}