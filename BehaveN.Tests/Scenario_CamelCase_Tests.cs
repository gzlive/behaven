using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_CamelCase_Tests
    {
        private bool givenFooWasInvoked;
        private bool whenBarWasInvoked;
        private bool thenBazWasInvoked;

        [Test]
        public void it_invokes_steps_defined_with_camel_case_names()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenFooWasInvoked = false;
            this.whenBarWasInvoked = false;
            this.thenBazWasInvoked = false;

            s.Given("foo");
            s.When("bar");
            s.Then("baz");
            s.Verify();

            this.givenFooWasInvoked.Should().Be.True();
            this.whenBarWasInvoked.Should().Be.True();
            this.thenBazWasInvoked.Should().Be.True();
        }

        [Test]
        public void it_invokes_steps_defined_with_camel_case_names_with_VerifyText()
        {
            var s = new Scenario();
            s.UseStepDefinitionsFrom(this);
            this.givenFooWasInvoked = false;
            this.whenBarWasInvoked = false;
            this.thenBazWasInvoked = false;

            s.VerifyText("given foo\r\n" +
                         "when bar\r\n" +
                         "then baz\r\n");

            this.givenFooWasInvoked.Should().Be.True();
            this.whenBarWasInvoked.Should().Be.True();
            this.thenBazWasInvoked.Should().Be.True();
        }

        public void GivenFoo()
        {
            this.givenFooWasInvoked = true;
        }

        public void WhenBar()
        {
            this.whenBarWasInvoked = true;
        }

        public void ThenBaz()
        {
            this.thenBazWasInvoked = true;
        }
    }
}
