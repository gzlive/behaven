using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class StepDefinition_Tests
    {
        [Test]
        public void it_matches_steps_without_punctuation()
        {
            var def = new StepDefinition(this, this.GetType().GetMethod("given_a_foo"));

            bool matches = def.Matches(new Step { Type = StepType.Given, Text = "given a foo" });

            matches.Should().Be.True();
        }

        [Test]
        public void it_matches_steps_with_punctuation()
        {
            var def = new StepDefinition(this, this.GetType().GetMethod("given_a_foo"));

            bool matches = def.Matches(new Step { Type = StepType.Given, Text = "give'n a: foo!" });

            matches.Should().Be.True();
        }

        [Test]
        public void it_does_not_remove_punctuation_from_inside_quoted_strings()
        {
            var def = new StepDefinition(this, this.GetType().GetMethod("given_the_string_arg1"));

            def.TryExecute(new Step { Type = StepType.Given, Text = "given the string \"a/b.c!\"" });

            this.theString.Should().Be("a/b.c!");
        }

        [Test]
        public void it_is_not_case_sensitive()
        {
            var def = new StepDefinition(this, this.GetType().GetMethod("given_a_foo"));

            bool matches = def.Matches(new Step { Type = StepType.Given, Text = "GIVEN A FOO" });

            matches.Should().Be.True();
        }

        public void given_a_foo()
        {
        }

        private string theString;

        public void given_the_string_arg1(string s)
        {
            this.theString = s;
        }
    }
}
