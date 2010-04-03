using NUnit.Framework;

namespace BehaveN.Tests
{
    public class BaseFeatureTests
    {
        [SetUp]
        public void SetUp()
        {
            _feature = new Feature();
        }

        private Feature _feature;

        protected Feature TheFeature { get { return _feature; } }

        protected void LoadText(params string[] lines)
        {
            string text = string.Join("\r\n", lines);
            new PlainTextReader().ReadTo(text, _feature);
        }
    }
}