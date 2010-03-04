using NUnit.Framework;

namespace BehaveN.Tests
{
    public class BaseSpecificationsFileTests
    {
        [SetUp]
        public void SetUp()
        {
            _specs = new SpecificationsFile();
        }

        private SpecificationsFile _specs;

        protected SpecificationsFile TheSpecificationsFile { get { return _specs; } }

        protected void LoadText(params string[] lines)
        {
            string text = string.Join("\r\n", lines);
            _specs.LoadText(text);
        }
    }
}