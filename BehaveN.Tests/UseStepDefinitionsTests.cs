using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class UseStepDefinitionsTests : Scenario
    {
        [SetUp]
        public void SetUp()
        {
            Steps1._step1Invoked = false;
            Steps2._step2Invoked = false;
        }

        [Test]
        public void UseStepDefinitionsFrom()
        {
            Scenario s = new Scenario();
            s.UseStepDefinitionsFrom(new Steps1());

            s.Given("Step1");
            s.Verify();

            Assert.That(Steps1._step1Invoked, Is.True);
        }

        [Test]
        public void UseStepDefinitionsFromType()
        {
            Scenario s = new Scenario();
            s.UseStepDefinitionsFromType<Steps1>();

            s.Given("Step1");
            s.Verify();

            Assert.That(Steps1._step1Invoked, Is.True);
        }

        [Test]
        public void UseStepDefinitionsFromAssemblyOfType()
        {
            Scenario s = new Scenario();
            s.UseStepDefinitionsFromAssemblyOfType<Steps1>();

            s.Given("Step1");
            s.Given("Step2");
            s.Verify();

            Assert.That(Steps1._step1Invoked, Is.True);
            Assert.That(Steps2._step2Invoked, Is.True);
        }

        public class Steps1
        {
            public static bool _step1Invoked;

            public void given_step1()
            {
                _step1Invoked = true;
            }
        }

        public class Steps2
        {
            public static bool _step2Invoked;

            public void given_step2()
            {
                _step2Invoked = true;
            }
        }
    }
}
