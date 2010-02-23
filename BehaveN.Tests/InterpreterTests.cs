using System;
using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class InterpreterTests
    {
        [Test]
        public void Test()
        {
            Interpreter1 i1 = new Interpreter1();
            Interpreter2 i2 = new Interpreter2();

            Specifications specifications = new Specifications();
            specifications.UseStepDefinitionsFrom(i1);
            specifications.UseStepDefinitionsFrom(i2);

            specifications.VerifyText("Given foo\r\n" +
                                "Given bar\r\n" +
                                "Given baz\r\n");

            Assert.That(i1._fooInvoked, Is.True);
            Assert.That(i1._barInvoked, Is.True);
            Assert.That(i2._barInvoked, Is.False);
            Assert.That(i2._bazInvoked, Is.True);
        }

        private class Interpreter1
        {
            public bool _fooInvoked;
            public bool _barInvoked;

            public void GivenFoo()
            {
                _fooInvoked = true;
            }

            public void GivenBar()
            {
                _barInvoked = true;
            }
        }

        private class Interpreter2
        {
            public bool _barInvoked;
            public bool _bazInvoked;

            [CoverageExclude]
            public void GivenBar()
            {
                _barInvoked = true;
            }

            public void GivenBaz()
            {
                _bazInvoked = true;
            }
        }
    }
}
