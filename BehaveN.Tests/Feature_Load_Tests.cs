using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Feature_Load_Tests : BaseFeatureTests
    {
        [Test]
        public void it_loads_empty_files()
        {
            LoadText();

            TheFeature.Scenarios.Count.Should().Be(0);
        }

        [Test]
        public void it_loads_empty_scenarios()
        {
            LoadText("Scenario: A",
                     "Scenario: B");

            TheFeature.Scenarios.Count.Should().Be(2);
            TheFeature.Scenarios[0].Name.Should().Be("A");
            TheFeature.Scenarios[1].Name.Should().Be("B");
        }

        [Test]
        public void it_loads_headers()
        {
            LoadText("# foo: bar",
                     "Scenario: A");

            TheFeature.Headers["foo"].Should().Be("bar");
        }

        [Test]
        public void it_loads_the_steps_in_a_scenario()
        {
            LoadText("Scenario: A",
                     "Given B",
                     "When C",
                     "Then D",
                     "And E");

            TheFeature.Scenarios.Count.Should().Be(1);
            TheFeature.Scenarios[0].Name.Should().Be("A");
            TheFeature.Scenarios[0].Steps.Count.Should().Be(4);
            TheFeature.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheFeature.Scenarios[0].Steps[1].Text.Should().Be("When C");
            TheFeature.Scenarios[0].Steps[2].Text.Should().Be("Then D");
            TheFeature.Scenarios[0].Steps[3].Text.Should().Be("And E");
        }

        [Test]
        public void it_ignores_comments()
        {
            LoadText("Scenario: A",
                     "Given B",
                     "When C",
                     "Then D",
                     "# Comment", 
                     "And E");

            TheFeature.Scenarios.Count.Should().Be(1);
            TheFeature.Scenarios[0].Name.Should().Be("A");
            TheFeature.Scenarios[0].Steps.Count.Should().Be(4);
            TheFeature.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheFeature.Scenarios[0].Steps[1].Text.Should().Be("When C");
            TheFeature.Scenarios[0].Steps[2].Text.Should().Be("Then D");
            TheFeature.Scenarios[0].Steps[3].Text.Should().Be("And E");
        }

        [Test]
        public void it_loads_steps_with_forms()
        {
            LoadText("Scenario: A",
                     "Given B",
                     "  : C : D",
                     "  : E : F",
                     "When G",
                     "Then H");

            TheFeature.Scenarios.Count.Should().Be(1);
            TheFeature.Scenarios[0].Name.Should().Be("A");
            TheFeature.Scenarios[0].Steps.Count.Should().Be(3);
            TheFeature.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheFeature.Scenarios[0].Steps[0].Block.Should().Be.InstanceOf<Form>();
            Form form = (Form)TheFeature.Scenarios[0].Steps[0].Block;
            form.Size.Should().Be(2);
            form.GetLabel(0).Should().Be("C");
            form.GetValue(0).Should().Be("D");
            form.GetLabel(1).Should().Be("E");
            form.GetValue(1).Should().Be("F");
            TheFeature.Scenarios[0].Steps[1].Text.Should().Be("When G");
            TheFeature.Scenarios[0].Steps[2].Text.Should().Be("Then H");
        }

        [Test]
        public void it_loads_steps_with_grids()
        {
            LoadText("Scenario: A",
                     "Given B",
                     "  | C | D |",
                     "  | E | F |",
                     "  | G | H |",
                     "When I",
                     "Then J");

            TheFeature.Scenarios.Count.Should().Be(1);
            TheFeature.Scenarios[0].Name.Should().Be("A");
            TheFeature.Scenarios[0].Steps.Count.Should().Be(3);
            TheFeature.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheFeature.Scenarios[0].Steps[0].Block.Should().Be.InstanceOf<Grid>();
            Grid grid = (Grid)TheFeature.Scenarios[0].Steps[0].Block;
            grid.ColumnCount.Should().Be(2);
            grid.RowCount.Should().Be(2);
            grid.GetHeader(0).Should().Be("C");
            grid.GetHeader(1).Should().Be("D");
            grid.GetValue(0, 0).Should().Be("E");
            grid.GetValue(0, 1).Should().Be("F");
            grid.GetValue(1, 0).Should().Be("G");
            grid.GetValue(1, 1).Should().Be("H");
            TheFeature.Scenarios[0].Steps[1].Text.Should().Be("When I");
            TheFeature.Scenarios[0].Steps[2].Text.Should().Be("Then J");
        }
    }
}
