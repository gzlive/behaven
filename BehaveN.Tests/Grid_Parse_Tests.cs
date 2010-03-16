using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;

namespace BehaveN.Tests
{
    [TestFixture]
    public class Grid_Parse_Tests
    {
        [Test]
        [Description("foo")]
        public void it_is_able_to_parse_a_grid_with_only_headers()
        {
            var grid = ParseGrid("| foo | bar | baz |");

            grid.ColumnCount.Should().Be(3);
            grid.RowCount.Should().Be(0);
        }

        [Test]
        public void it_is_able_to_parse_a_grid_with_both_headers_and_values()
        {
            var grid = ParseGrid("| foo | bar | baz |\r\n"
                               + "|  a  |  b  |  c  |\r\n"
                               + "|  d  |  e  |  f  |");

            grid.ColumnCount.Should().Be(3);
            grid.RowCount.Should().Be(2);
        }

        [Test]
        public void it_strips_spaces_off_the_ends_of_the_header_names()
        {
            var grid = ParseGrid("| foo | bar | baz |");

            grid.ColumnCount.Should().Be(3);
            grid.GetHeader(0).Should().Be("foo");
            grid.GetHeader(1).Should().Be("bar");
            grid.GetHeader(2).Should().Be("baz");
        }

        [Test]
        public void it_strips_spaces_off_the_ends_of_the_values()
        {
            var grid = ParseGrid("| foo | bar | baz |\r\n"
                               + "|  a  |  b  |  c  |");

            grid.RowCount.Should().Be(1);
            grid.GetValue(0, 0).Should().Be("a");
            grid.GetValue(0, 1).Should().Be("b");
            grid.GetValue(0, 2).Should().Be("c");
        }

        private Grid ParseGrid(string text)
        {
            var gridBlockType = BlockTypes.GetBlockTypeFor(typeof(List<MyObject>));
            return (Grid)gridBlockType.Parse(text);
        }
    }
}
