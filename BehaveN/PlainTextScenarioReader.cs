using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Reads specifications in the Given/When/Then style from a plain text file.
    /// </summary>
    public class PlainTextScenarioReader
    {
        private readonly string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextScenarioReader"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public PlainTextScenarioReader(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Reads the specifications to the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public void ReadTo(Scenario scenario)
        {
            List<string> lines = TextParser.GetLines(_text);

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                IConvertibleObject convertibleObject = null;

                if (Form.NextLineIsForm(lines, i))
                {
                    Form form = Form.ParseForm(lines, ++i);

                    i += form.Size - 1;

                    convertibleObject = form;
                }
                else if (Grid.NextLineIsGrid(lines, i))
                {
                    Grid grid = Grid.ParseGrid(lines, ++i);

                    i += grid.RowCount;

                    convertibleObject = grid;
                }

                ParseLine(line, convertibleObject, scenario);
            }
        }

        private static readonly Regex _scenarioRegex = new Regex(@"^\s*Scenario\s*\d*\s*:\s*(.+)", RegexOptions.IgnoreCase);
        private static readonly Regex _givenRegex = new Regex(@"^\s*Given\s+(.+)", RegexOptions.IgnoreCase);
        private static readonly Regex _whenRegex = new Regex(@"^\s*When\s+(.+)", RegexOptions.IgnoreCase);
        private static readonly Regex _thenRegex = new Regex(@"^\s*Then\s+(.+)", RegexOptions.IgnoreCase);
        private static readonly Regex _andRegex = new Regex(@"^\s*And\s+(.+)", RegexOptions.IgnoreCase);

        private void ParseLine(string line, IConvertibleObject convertibleObject, Scenario scenario)
        {
            Match m = _scenarioRegex.Match(line);

            if (m.Success)
            {
                scenario.Name(m.Groups[1].Value);
            }
            else
            {
                m = _givenRegex.Match(line);

                if (m.Success)
                {
                    scenario.Given(m.Groups[1].Value, convertibleObject);
                }
                else
                {
                    m = _whenRegex.Match(line);

                    if (m.Success)
                    {
                        scenario.When(m.Groups[1].Value, convertibleObject);
                    }
                    else
                    {
                        m = _thenRegex.Match(line);

                        if (m.Success)
                        {
                            scenario.Then(m.Groups[1].Value, convertibleObject);
                        }
                        else
                        {
                            m = _andRegex.Match(line);

                            if (m.Success)
                            {
                                scenario.And(m.Groups[1].Value, convertibleObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
