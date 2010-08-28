using NUnit.Framework;

namespace BehaveN.Examples
{
    [TestFixture]
    public class NUnitExample
    {
        [Test]
        public void Test()
        {
            Verify.EmbeddedResource(
                GetType().Assembly,
                "CalculatorScenarios.txt",
                GetType().Assembly);
        }
    }
}
