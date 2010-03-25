using System;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class SpecificationsFile_Load_Tests : BaseSpecificationsFileTests
    {
        [Test]
        public void it_loads_empty_files()
        {
            LoadText();

            TheSpecificationsFile.Scenarios.Count.Should().Be(0);
        }

        [Test]
        public void it_loads_empty_scenarios()
        {
            LoadText("Scenario: A",
                     "Scenario: B");

            TheSpecificationsFile.Scenarios.Count.Should().Be(2);
            TheSpecificationsFile.Scenarios[0].Name.Should().Be("A");
            TheSpecificationsFile.Scenarios[1].Name.Should().Be("B");
        }

        [Test]
        public void it_loads_headers()
        {
            LoadText("# foo: bar",
                     "Scenario: A");

            TheSpecificationsFile.Headers["foo"].Should().Be("bar");
        }

        [Test]
        public void it_loads_the_steps_in_a_scenario()
        {
            LoadText("Scenario: A",
                     "Given B",
                     "When C",
                     "Then D",
                     "And E");

            TheSpecificationsFile.Scenarios.Count.Should().Be(1);
            TheSpecificationsFile.Scenarios[0].Name.Should().Be("A");
            TheSpecificationsFile.Scenarios[0].Steps.Count.Should().Be(4);
            TheSpecificationsFile.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheSpecificationsFile.Scenarios[0].Steps[1].Text.Should().Be("When C");
            TheSpecificationsFile.Scenarios[0].Steps[2].Text.Should().Be("Then D");
            TheSpecificationsFile.Scenarios[0].Steps[3].Text.Should().Be("And E");
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

            TheSpecificationsFile.Scenarios.Count.Should().Be(1);
            TheSpecificationsFile.Scenarios[0].Name.Should().Be("A");
            TheSpecificationsFile.Scenarios[0].Steps.Count.Should().Be(4);
            TheSpecificationsFile.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheSpecificationsFile.Scenarios[0].Steps[1].Text.Should().Be("When C");
            TheSpecificationsFile.Scenarios[0].Steps[2].Text.Should().Be("Then D");
            TheSpecificationsFile.Scenarios[0].Steps[3].Text.Should().Be("And E");
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

            TheSpecificationsFile.Scenarios.Count.Should().Be(1);
            TheSpecificationsFile.Scenarios[0].Name.Should().Be("A");
            TheSpecificationsFile.Scenarios[0].Steps.Count.Should().Be(3);
            TheSpecificationsFile.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheSpecificationsFile.Scenarios[0].Steps[0].Block.Should().Be.InstanceOf<Form>();
            Form form = (Form)TheSpecificationsFile.Scenarios[0].Steps[0].Block;
            form.Size.Should().Be(2);
            form.GetLabel(0).Should().Be("C");
            form.GetValue(0).Should().Be("D");
            form.GetLabel(1).Should().Be("E");
            form.GetValue(1).Should().Be("F");
            TheSpecificationsFile.Scenarios[0].Steps[1].Text.Should().Be("When G");
            TheSpecificationsFile.Scenarios[0].Steps[2].Text.Should().Be("Then H");
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

            TheSpecificationsFile.Scenarios.Count.Should().Be(1);
            TheSpecificationsFile.Scenarios[0].Name.Should().Be("A");
            TheSpecificationsFile.Scenarios[0].Steps.Count.Should().Be(3);
            TheSpecificationsFile.Scenarios[0].Steps[0].Text.Should().Be("Given B");
            TheSpecificationsFile.Scenarios[0].Steps[0].Block.Should().Be.InstanceOf<Grid>();
            Grid grid = (Grid)TheSpecificationsFile.Scenarios[0].Steps[0].Block;
            grid.ColumnCount.Should().Be(2);
            grid.RowCount.Should().Be(2);
            grid.GetHeader(0).Should().Be("C");
            grid.GetHeader(1).Should().Be("D");
            grid.GetValue(0, 0).Should().Be("E");
            grid.GetValue(0, 1).Should().Be("F");
            grid.GetValue(1, 0).Should().Be("G");
            grid.GetValue(1, 1).Should().Be("H");
            TheSpecificationsFile.Scenarios[0].Steps[1].Text.Should().Be("When I");
            TheSpecificationsFile.Scenarios[0].Steps[2].Text.Should().Be("Then J");
        }
    }
}
