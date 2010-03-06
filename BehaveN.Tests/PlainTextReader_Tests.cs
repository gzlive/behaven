using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class PlainTextReader_Tests
    {
        private PlainTextReader _reader;
        private SpecificationsFile _specs;
        private Exception _exception;

        [Test]
        public void it_throws_an_exception_when_a_step_appears_before_a_scenario()
        {
            ReadText("Given foo");

            _exception.Should().Be.InstanceOf<Exception>();
        }

        [Test]
        public void it_throws_an_exception_when_an_and_step_appears_first_in_a_scenario()
        {
            ReadText("Scenario: No ands first",
                     "And foo");

            _exception.Should().Be.InstanceOf<Exception>();
        }

        [Test]
        public void it_throws_an_exception_when_it_encounters_a_step_it_does_not_recognize()
        {
            ReadText("Scenario: Unrecognized step",
                     "Foo bar");

            _exception.Should().Be.InstanceOf<Exception>();
        }

        [Test]
        public void it_parses_the_title_and_description_from_the_beginning_of_the_file()
        {
            ReadText("Feature: My feature",
                     "",
                     "This is a description of my feature.",
                     "This is the second line.",
                     "This next line starts with Given:",
                     "Given nothing because this is in a feature.",
                     "This is the end of my feature.",
                     "",
                     "Scenario: My scenario",
                     "Given foo");

            _specs.Title.Should().Be("My feature");
            _specs.Description.Should().Be(string.Join("\r\n", new[]
                                                                   {
                                                                       "This is a description of my feature.",
                                                                       "This is the second line.",
                                                                       "This next line starts with Given:",
                                                                       "Given nothing because this is in a feature.",
                                                                       "This is the end of my feature.",
                                                                   }));
            _specs.Scenarios.Count.Should().Be(1);
            _specs.Scenarios[0].Name.Should().Be("My scenario");
            _specs.Scenarios[0].Steps.Count.Should().Be(1);
            _specs.Scenarios[0].Steps[0].Text.Should().Be("Given foo");
        }

        [Test]
        public void it_parses_the_description_from_the_beginning_of_the_file_even_without_the_feature_header()
        {
            ReadText("This is a description of my feature.",
                     "This is the second line.",
                     "This next line starts with Given:",
                     "Given nothing because this is in a feature.",
                     "This is the end of my feature.",
                     "",
                     "Scenario: My scenario",
                     "Given foo");

            _specs.Title.Should().Be.Null();
            _specs.Description.Should().Be(string.Join("\r\n", new[]
                                                                   {
                                                                       "This is a description of my feature.",
                                                                       "This is the second line.",
                                                                       "This next line starts with Given:",
                                                                       "Given nothing because this is in a feature.",
                                                                       "This is the end of my feature.",
                                                                   }));
            _specs.Scenarios.Count.Should().Be(1);
            _specs.Scenarios[0].Name.Should().Be("My scenario");
            _specs.Scenarios[0].Steps.Count.Should().Be(1);
            _specs.Scenarios[0].Steps[0].Text.Should().Be("Given foo");
        }

        private void ReadText(params string[] lines)
        {
            _reader = new PlainTextReader(string.Join("\r\n", lines));
            _specs = new SpecificationsFile();
            _exception = null;

            try
            {
                _reader.ReadTo(_specs);
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }
    }
}
