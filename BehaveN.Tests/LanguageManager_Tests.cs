using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class LanguageManager_Tests
    {
        [Test]
        public void it_returns_strings_for_english()
        {
            var man = new LanguageManager();

            string s = man.GetString("en", "Given");

            s.Should().Be("Given");
        }

        [Test]
        public void it_returns_strings_for_latvian()
        {
            var man = new LanguageManager();

            string s = man.GetString("lv", "Given");

            s.Should().Be("Kad");
        }

        [Test]
        public void it_returns_strings_for_unknown_languages_in_english()
        {
            var man = new LanguageManager();

            string s = man.GetString("xx", "Given");

            s.Should().Be("Given");
        }
    }
}
