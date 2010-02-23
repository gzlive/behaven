using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Scenario_Underscores_Tests
    {
        private bool givenFooWasInvoked;
        private bool whenBarWasInvoked;
        private bool thenBazWasInvoked;

        [Test]
        public void it_invokes_steps_defined_with_underscores_in_their_name()
        {
            var s = new Specifications();
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
        public void it_invokes_steps_defined_with_underscores_in_their_name_with_VerifyText()
        {
            var s = new Specifications();
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

        public void given_foo()
        {
            this.givenFooWasInvoked = true;
        }

        public void when_bar()
        {
            this.whenBarWasInvoked = true;
        }

        public void then_baz()
        {
            this.thenBazWasInvoked = true;
        }
    }
}
