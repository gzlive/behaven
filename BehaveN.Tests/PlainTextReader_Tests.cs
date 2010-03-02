using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class PlainTextReader_Tests
    {
        private PlainTextReader reader;
        private FeatureFile ff;
        private Exception exception;

        [Test]
        public void it_throws_an_exception_when_a_step_appears_before_a_scenario()
        {
            ReadText("Given foo");

            exception.Should().Be.InstanceOf<Exception>();
        }

        [Test]
        public void it_throws_an_exception_when_an_and_step_appears_first_in_a_scenario()
        {
            ReadText("Scenario: No ands first",
                     "And foo");

            exception.Should().Be.InstanceOf<Exception>();
        }

        [Test]
        public void it_throws_an_exception_when_it_encounters_a_step_it_does_not_recognize()
        {
            ReadText("Scenario: Unrecognized step",
                     "Foo bar");

            exception.Should().Be.InstanceOf<Exception>();
        }

        private void ReadText(params string[] lines)
        {
            reader = new PlainTextReader(string.Join("\r\n", lines));
            ff = new FeatureFile();
            exception = null;

            try
            {
                reader.ReadTo(ff);
            }
            catch (Exception e)
            {
                exception = e;
            }
        }
    }
}
