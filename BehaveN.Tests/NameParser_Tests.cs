using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class NameParser_Tests
    {
        [Test]
        public void it_throws_an_exception_when_parsing_null()
        {
            (new Action(() => NameParser.Parse((string)null))).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void it_returns_an_empty_string_when_parsing_an_empty_string()
        {
            string result = NameParser.Parse("");

            result.Should().Be("");
        }

        [Test]
        public void it_returns_the_same_string_when_parsing_a_single_word()
        {
            string result = NameParser.Parse("foo");

            result.Should().Be("foo");
        }

        [Test]
        public void it_returns_two_strings_when_parsing_words_separated_by_semicolons()
        {
            string result = NameParser.Parse("foo_bar");

            result.Should().Be("foo bar");
        }

        [Test]
        public void it_returns_two_strings_when_parsing_a_words_using_pascal_case()
        {
            string result = NameParser.Parse("FooBar");

            result.Should().Be("Foo Bar");
        }
    }
}
