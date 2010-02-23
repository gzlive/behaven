using System.IO;
using NUnit.Framework;

namespace BehaveN.Tests
{
    [TestFixture]
    public class FileTests : Specifications
    {
        private bool _givenInvoked;
        private bool _whenInvoked;
        private bool _thenInvoked;

        [Test]
        public void Test()
        {
            string tempPath = Path.GetTempFileName();

            File.WriteAllText(tempPath,
                              "Given some context\r\n" +
                              "When some event occurs\r\n" +
                              "Then some outcome should be true\r\n");

            VerifyFile(tempPath);

            File.Delete(tempPath);

            Assert.That(_givenInvoked, Is.True);
            Assert.That(_whenInvoked, Is.True);
            Assert.That(_thenInvoked, Is.True);
        }

        public void GivenSomeContext()
        {
            _givenInvoked = true;
        }

        public void WhenSomeEventOccurs()
        {
            _whenInvoked = true;
        }

        public void ThenSomeOutcomeShouldBeTrue()
        {
            _thenInvoked = true;
        }
    }
}
