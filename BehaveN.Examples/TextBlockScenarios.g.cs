// This code was generated by the BehaveN tool.

using BehaveN;
using NUnit.Framework;
using System.Reflection;

namespace BehaveN.Examples
{
    [TestFixture]
    public partial class TextBlockScenarios
    {
        private Feature _feature = new Feature();

        [TestFixtureSetUp]
        public void LoadScenarios()
        {
            _feature.StepDefinitions.UseStepDefinitionsFromAssembly(GetType().Assembly);
            _feature.ReadEmbeddedResource(GetType().Assembly, "TextBlockScenarios.txt");
        }

        [TestFixtureTearDown]
        public void ReportUndefinedSteps()
        {
            _feature.ReportUndefinedSteps();
        }

        [Test]
        public void Text()
        {
            _feature.Scenarios["Text"].Verify();
        }
    }
}
