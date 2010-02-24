using NUnit.Framework;

namespace BehaveN.Tests
{
    public class BaseFeatureFileTests
    {
        [SetUp]
        public void SetUp()
        {
            _ff = new FeatureFile();
        }

        private FeatureFile _ff;

        protected FeatureFile TheFeatureFile { get { return _ff; } }

        protected void LoadText(params string[] lines)
        {
            string text = string.Join("\r\n", lines);
            _ff.LoadText(text);
        }
    }
}