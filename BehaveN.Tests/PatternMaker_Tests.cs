using System;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class PatternMaker_Tests
    {
        [Test]
        public void it_generates_patterns_for_int_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_an_int_n");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "an int 123");
            groups["n"].Value.Should().Be("123");
        }

        [Test]
        public void it_generates_patterns_that_support_negatives_for_int_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_an_int_n");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "an int -123");
            groups["n"].Value.Should().Be("-123");
        }

        public void given_an_int_n(int n)
        {
        }

        [Test]
        public void it_generates_patterns_for_decimal_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_decimal_d");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a decimal 123.456");
            groups["d"].Value.Should().Be("123.456");
        }

        [Test]
        public void it_generates_patterns_that_support_negatives_for_decimal_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_decimal_d");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a decimal -123.456");
            groups["d"].Value.Should().Be("-123.456");
        }

        [Test]
        public void it_generates_patterns_that_support_dollar_signs_for_decimal_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_decimal_d");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a decimal $123.456");
            groups["d"].Value.Should().Be("123.456");
        }

        [Test]
        public void it_generates_patterns_that_do_not_require_dots_for_decimal_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_decimal_d");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a decimal 123");
            groups["d"].Value.Should().Be("123");
        }

        public void given_a_decimal_d(decimal d)
        {
        }

        [Test]
        public void it_generates_patterns_for_string_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_string_s");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a string foo bar baz");
            groups["s"].Value.Should().Be("foo bar baz");
        }

        [Test]
        public void it_generates_patterns_that_support_quotes_for_string_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_string_s");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a string \"foo bar baz\"");
            groups["s"].Value.Should().Be("foo bar baz");
        }

        public void given_a_string_s(string s)
        {
        }

        [Test]
        public void it_generates_patterns_for_date_time_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_date_time_dt");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a date time 2009-10-16");
            groups["dt"].Value.Should().Be("2009-10-16");
        }

        public void given_a_date_time_dt(DateTime dt)
        {
        }

        [Test]
        public void it_generates_patterns_for_enum_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_an_enum_e");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "an enum Foo");
            groups["e"].Value.Should().Be("Foo");
        }

        [Test]
        public void it_generates_patterns_that_support_spaces_for_enum_arguments()
        {
            MethodInfo method = this.GetType().GetMethod("given_an_enum_e");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "an enum Baz Quux");
            groups["e"].Value.Should().Be("Baz Quux");
        }

        [Test]
        public void it_generates_patterns_that_do_not_match_when_an_enum_argument_is_wrong()
        {
            MethodInfo method = this.GetType().GetMethod("given_an_enum_e");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "an enum abc");
            groups["e"].Value.Should().Be("");
        }

        public enum MyEnum { Foo, Bar, BazQuux }

        public void given_an_enum_e(MyEnum e)
        {
        }

        [Test]
        public void it_generates_patterns_for_methods_with_two_arguments_in_the_same_order_as_the_method_name()
        {
            MethodInfo method = this.GetType().GetMethod("given_foo_fooName_and_bar_barName");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "foo my foo and bar my bar");
            groups["fooName"].Value.Should().Be("my foo");
            groups["barName"].Value.Should().Be("my bar");
        }

        public void given_foo_fooName_and_bar_barName(string fooName, string barName)
        {
        }

        [Test]
        public void it_generates_patterns_for_methods_with_two_arguments_not_in_the_same_order_as_the_method_name()
        {
            MethodInfo method = this.GetType().GetMethod("given_bar_barName_and_foo_fooName");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "bar my bar and foo my foo");
            groups["fooName"].Value.Should().Be("my foo");
            groups["barName"].Value.Should().Be("my bar");
        }

        public void given_bar_barName_and_foo_fooName(string fooName, string barName)
        {
        }

        [Test]
        public void it_allows_extra_spaces_in_the_step_an_the_beginning()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_whatever");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "  a whatever");
            groups[0].Success.Should().Be.True();
        }

        [Test]
        public void it_allows_extra_spaces_in_the_step_in_the_middle()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_whatever");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a  whatever");
            groups[0].Success.Should().Be.True();
        }

        [Test]
        public void it_allows_extra_spaces_in_the_step_an_the_end()
        {
            MethodInfo method = this.GetType().GetMethod("given_a_whatever");

            string pattern = PatternMaker.GetPattern(method);

            var groups = ApplyPattern(pattern, "a whatever  ");
            groups[0].Success.Should().Be.True();
        }

        public void given_a_whatever()
        {
        }

        private static GroupCollection ApplyPattern(string pattern, string text)
        {
            var regex = new Regex("^" + pattern + "$");
            var match = regex.Match(text);
            return match.Groups;
        }
    }
}
