using NUnit.Framework;
using System.Reflection;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class NewTests
    {
        private bool givenInvoked;
        private bool whenInvoked;
        private bool thenInvoked;

        [Test]
        public void GivenUnderscore()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("foo");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void WhenUnderscore()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.whenInvoked = false;

            s.When("foo");
            s.Verify();

            this.whenInvoked.Should().Be.True();
        }

        [Test]
        public void ThenUnderscore()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.thenInvoked = false;

            s.Then("foo");
            s.Verify();

            this.thenInvoked.Should().Be.True();
        }

        [Test]
        public void GivenWhenThenInText()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;
            this.whenInvoked = false;
            this.thenInvoked = false;

            s.VerifyText(
                  "Given foo\r\n"
                + "When foo\r\n"
                + "Then foo\r\n");

            this.givenInvoked.Should().Be.True();
            this.whenInvoked.Should().Be.True();
            this.thenInvoked.Should().Be.True();
        }

        [Test]
        public void GivenCamelCase()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("bar");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void IntArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the int 123");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void Int1stArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the int 1st");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void Int2ndArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the int 2nd");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void Int3rdArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the int 3rd");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void Int4thArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the int 4th");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void IntAndDecimalArgs()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the int 123 and the decimal 456.78");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void DecimalAndIntArgsDeclaredInDifferentOrder()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the decimal 123.45 and the int 678");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void DecimalArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the decimal 123.45");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void DecimalWithoutPointArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the decimal 123");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void DecimalWithDollarSignArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the decimal $123.45");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void DecimalWithDollarSignAndWithoutPointArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the decimal $123");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void StringArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the string \"foo\"");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void StringWithoutQuotesArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the string foo");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void StringWithSpaceAndWithoutQuotesAndIntArgs()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the string foo bar and int 123");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void EnumArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the enum foo");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void EnumWithSpacesArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the enum baz quux");
            s.Verify();

            this.givenInvoked.Should().Be.True();
        }

        [Test]
        public void BadEnumArg()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenInvoked = false;

            s.Given("the enum quuux");
            Assert.Throws(typeof(VerificationException), s.Verify);

            this.givenInvoked.Should().Be.False();
        }

        [Test]
        public void PatternForIntArgument()
        {
            MethodInfo methodInfo = this.GetType().GetMethod("given_the_int_n");

            string pattern = PatternMaker.GetPattern(methodInfo);

            pattern.Should().Be(@"\s*the\s+int\s+(?<n>-?\d+)(?:st|nd|rd|th)?\s*");
        }

        [Test]
        public void PatternForEnumArgument()
        {
            MethodInfo methodInfo = this.GetType().GetMethod("given_the_enum_e");

            string pattern = PatternMaker.GetPattern(methodInfo);

            // Reflection isn't required to return the fields back in the order they're declared...
            pattern.Should().Be(@"\s*the\s+enum\s+(?<e>(?:Foo)|(?:Bar)|(?:Baz\s*Quux))\s*");
        }

        public void given_foo()
        {
            this.givenInvoked = true;
        }

        public void when_foo()
        {
            this.whenInvoked = true;
        }

        public void then_foo()
        {
            this.thenInvoked = true;
        }

        public void GivenBar()
        {
            this.givenInvoked = true;
        }

        public void given_the_int_n(int n)
        {
            this.givenInvoked = true;
        }

        public void given_the_decimal_d(decimal d)
        {
            this.givenInvoked = true;
        }

        public void given_the_string_s(string s)
        {
            this.givenInvoked = true;
        }

        public void given_the_string_s_and_int_n(string s, int n)
        {
            this.givenInvoked = true;
        }

        public void given_the_int_n_and_the_decimal_d(int n, decimal d)
        {
            this.givenInvoked = true;
        }

        public void given_the_decimal_d_and_the_int_n(int n, decimal d)
        {
            this.givenInvoked = true;
        }

        public void given_the_enum_e(TestEnum e)
        {
            this.givenInvoked = true;
        }
    }
}
