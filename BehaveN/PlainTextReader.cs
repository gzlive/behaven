using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Reads specifications in the Given/When/Then style from a plain text file.
    /// </summary>
    public class PlainTextReader
    {
        private readonly string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextReader"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public PlainTextReader(string text)
        {
            _text = text;
        }

        public void ReadTo(FeatureFile featureFile)
        {
            List<string> lines = TextParser.GetLines(_text);

            Scenario scenario = null;
            string keyword = null;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                Match m = _scenarioRegex.Match(line);

                if (m.Success)
                {
                    scenario = new Scenario();
                    scenario.Name = m.Groups[1].Value;
                    featureFile.Scenarios.Add(scenario);
                }
                else
                {
                    ParseStep(lines, scenario, ref keyword, ref i, line);
                }
            }
        }

        private void ParseStep(List<string> lines, Scenario scenario, ref string keyword, ref int i, string line)
        {
            Match m = _gwtRegex.Match(line);

            if (m.Success)
            {
                keyword = m.Groups[1].Value.ToLower();
            }
            else
            {
                m = _andRegex.Match(line);
            }

            if (scenario == null)
                throw new Exception("Steps cannot appear before a scenario is started.");

            IBlock block = ParseBlock(lines, ref i);

            scenario.Add(keyword, line, block);
        }

        private IBlock ParseBlock(List<string> lines, ref int i)
        {
            IBlock block = null;

            if (Form.NextLineIsForm(lines, i))
            {
                Form form = Form.ParseForm(lines, ++i);
                i += form.Size - 1;
                block = form;
            }
            else if (Grid.NextLineIsGrid(lines, i))
            {
                Grid grid = Grid.ParseGrid(lines, ++i);
                i += grid.RowCount;
                block = grid;
            }

            return block;
        }

        private static readonly Regex _scenarioRegex = new Regex(@"^\s*Scenario\s*\d*\s*:\s*(.+)", RegexOptions.IgnoreCase);
        private static readonly Regex _gwtRegex = new Regex(@"^\s*(Given|When|Then)\s+.+", RegexOptions.IgnoreCase);
        private static readonly Regex _andRegex = new Regex(@"^\s*(And)\s+.+", RegexOptions.IgnoreCase);
    }
}
