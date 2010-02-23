using System;
using System.IO;
using NUnit.Framework;

namespace BehaveN.Tests
{
//    [TestFixture]
//    public class ScenarioOverloadsTests : Scenario
//    {
//        private StringWriter _stringWriter;
//        private int _action0;
//        private int _action1;
//        private int _action2;
//        private int _action3;
//        private int _action4;
//
//        [SetUp]
//        public void SetUp()
//        {
//            _stringWriter = new StringWriter();
//            Reporter = new PlainTextScenarioReporter(_stringWriter);
//            _action0 = 0;
//            _action1 = 0;
//            _action2 = 0;
//            _action3 = 0;
//            _action4 = 0;
//        }
//
//        [Test]
//        public void Test()
//        {
//            Assert.Throws(typeof(InvalidOperationException), delegate { And("action0"); });
//
//            Given("action0");
//            And("action0");
//
//            When("action0");
//            And("action0");
//
//            Then("action0");
//            And("action0");
//
//            Given(action0);
//            Given(action1, 0);
//            Given(action2, 0, "");
//            Given(action3, 0, "", 0m);
//            Given(action4, 0, "", 0m, DateTime.MinValue);
//
//            And(action0);
//            And(action1, 0);
//            And(action2, 0, "");
//            And(action3, 0, "", 0m);
//            And(action4, 0, "", 0m, DateTime.MinValue);
//
//            When(action0);
//            When(action1, 0);
//            When(action2, 0, "");
//            When(action3, 0, "", 0m);
//            When(action4, 0, "", 0m, DateTime.MinValue);
//
//            And(action0);
//            And(action1, 0);
//            And(action2, 0, "");
//            And(action3, 0, "", 0m);
//            And(action4, 0, "", 0m, DateTime.MinValue);
//
//            Then(action0);
//            Then(action1, 0);
//            Then(action2, 0, "");
//            Then(action3, 0, "", 0m);
//            Then(action4, 0, "", 0m, DateTime.MinValue);
//
//            And(action0);
//            And(action1, 0);
//            And(action2, 0, "");
//            And(action3, 0, "", 0m);
//            And(action4, 0, "", 0m, DateTime.MinValue);
//
//            Add(given_action0);
//            Add(given_action1, 0);
//            Add(given_action2, 0, "");
//            Add(given_action3, 0, "", 0m);
//            Add(given_action4, 0, "", 0m, DateTime.MinValue);
//
//            Add(when_action0);
//            Add(when_action1, 0);
//            Add(when_action2, 0, "");
//            Add(when_action3, 0, "", 0m);
//            Add(when_action4, 0, "", 0m, DateTime.MinValue);
//
//            Add(then_action0);
//            Add(then_action1, 0);
//            Add(then_action2, 0, "");
//            Add(then_action3, 0, "", 0m);
//            Add(then_action4, 0, "", 0m, DateTime.MinValue);
//
//            Assert.Throws(typeof(ArgumentException), () => Add(action0));
//
//            Verify();
//
//            Assert.That(_action0, Is.EqualTo(15));
//            Assert.That(_action1, Is.EqualTo(9));
//            Assert.That(_action2, Is.EqualTo(9));
//            Assert.That(_action3, Is.EqualTo(9));
//            Assert.That(_action4, Is.EqualTo(9));
//        }
//
//        [Step]
//        public void action0()
//        {
//            _action0++;
//        }
//
//        [Step]
//        public void action1(int i)
//        {
//            _action1++;
//        }
//
//        [Step]
//        public void action2(int i, string s)
//        {
//            _action2++;
//        }
//
//        [Step]
//        public void action3(int i, string s, decimal d)
//        {
//            _action3++;
//        }
//
//        [Step]
//        public void action4(int i, string s, decimal d, DateTime dt)
//        {
//            _action4++;
//        }
//
//        public void given_action0()
//        {
//            _action0++;
//        }
//
//        public void when_action0()
//        {
//            _action0++;
//        }
//
//        public void then_action0()
//        {
//            _action0++;
//        }
//
//        public void given_action1(int i)
//        {
//            _action1++;
//        }
//
//        public void when_action1(int i)
//        {
//            _action1++;
//        }
//
//        public void then_action1(int i)
//        {
//            _action1++;
//        }
//
//        public void given_action2(int i, string s)
//        {
//            _action2++;
//        }
//
//        public void when_action2(int i, string s)
//        {
//            _action2++;
//        }
//
//        public void then_action2(int i, string s)
//        {
//            _action2++;
//        }
//
//        public void given_action3(int i, string s, decimal d)
//        {
//            _action3++;
//        }
//
//        public void when_action3(int i, string s, decimal d)
//        {
//            _action3++;
//        }
//
//        public void then_action3(int i, string s, decimal d)
//        {
//            _action3++;
//        }
//
//        public void given_action4(int i, string s, decimal d, DateTime dt)
//        {
//            _action4++;
//        }
//
//        public void when_action4(int i, string s, decimal d, DateTime dt)
//        {
//            _action4++;
//        }
//
//        public void then_action4(int i, string s, decimal d, DateTime dt)
//        {
//            _action4++;
//        }
//    }
}
