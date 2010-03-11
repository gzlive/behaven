// <copyright file="PlainTextReader.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Reads specifications in the Given/When/Then style from a plain text file.
    /// </summary>
    public class PlainTextReader
    {
        private Match _match;

        /// <summary>
        /// Reads the contents of the file to the specifications file.
        /// </summary>
        /// <param name="text">The text to read.</param>
        /// <param name="specificationsFile">The specifications file.</param>
        public void ReadTo(string text, SpecificationsFile specificationsFile)
        {
            CompileRegexes(text);

            List<string> lines = TextParser.GetLines(text);

            Scenario scenario = null;
            StepType stepType = StepType.Unknown;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (scenario == null)
                {
                    i = ParseTitleAndDescription(lines, i, specificationsFile);
                }
                else
                {
                    _match = _scenarioRegex.Match(line);
                }

                if (_match.Success)
                {
                    scenario = new Scenario();
                    scenario.Name = _match.Groups[1].Value;
                    specificationsFile.Scenarios.Add(scenario);
                    stepType = StepType.Unknown;
                }
                else
                {
                    ParseStep(lines, ref i, scenario, ref stepType, line);
                }
            }
        }

        private int ParseTitleAndDescription(List<string> lines, int i, SpecificationsFile specificationsFile)
        {
            string line = lines[i];

            if ((_match = _featureRegex.Match(line)).Success) 
            {
                specificationsFile.Title = _match.Groups[1].Value;
                i++;
            }

            List<string> featureLines = new List<string>();

            for (; i < lines.Count; i++)
            {
                line = lines[i];

                _match = _scenarioRegex.Match(line);

                if (_match.Success)
                {
                    break;
                }

                featureLines.Add(line);
            }

            specificationsFile.Description = string.Join("\r\n", featureLines.ToArray()).Trim('\r', '\n');

            return i;
        }

        private void ParseStep(List<string> lines, ref int i, Scenario scenario, ref StepType stepType, string line)
        {
            if (_givenRegex.Match(line).Success)
            {
                stepType = StepType.Given;
            }
            else if (_whenRegex.Match(line).Success)
            {
                stepType = StepType.When;
            }
            else if (_thenRegex.Match(line).Success)
            {
                stepType = StepType.Then;
            }
            else if (_andRegex.Match(line).Success)
            {
                if (stepType == StepType.Unknown)
                    throw new Exception("\"And\" steps cannot appear before \"given\", \"when\", or \"then\" steps.");
            }
            else
            {
                throw new Exception(string.Format("Unrecognized step: \"{0}\".", line));
            }

            IBlock block = ParseBlock(lines, ref i);

            scenario.Steps.Add(stepType, line, block);
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

        private void CompileRegexes(string text)
        {
            var lm = new LanguageManager();

            string language = TextParser.DiscoverLanguage(text);

            string feature = lm.GetString(language, "Feature");
            string scenario = lm.GetString(language, "Scenario");
            string given = lm.GetString(language, "Given");
            string when = lm.GetString(language, "When");
            string then = lm.GetString(language, "Then");
            string and = lm.GetString(language, "And");

            _featureRegex = new Regex(string.Format(_featurePattern, feature), RegexOptions.IgnoreCase);
            _scenarioRegex = new Regex(string.Format(_scenarioPattern, scenario), RegexOptions.IgnoreCase);
            _givenRegex = new Regex(string.Format(_stepPattern, given), RegexOptions.IgnoreCase);
            _whenRegex = new Regex(string.Format(_stepPattern, when), RegexOptions.IgnoreCase);
            _thenRegex = new Regex(string.Format(_stepPattern, then), RegexOptions.IgnoreCase);
            _andRegex = new Regex(string.Format(_stepPattern, and), RegexOptions.IgnoreCase);
        }

        private CultureInfo GetCultureInfo(string language)
        {
            try
            {
                return CultureInfo.GetCultureInfo(language);
            }
            catch (ArgumentException)
            {
                return CultureInfo.GetCultureInfo("en");
            }
        }

        private Regex _featureRegex;
        private Regex _scenarioRegex;
        private Regex _givenRegex;
        private Regex _whenRegex;
        private Regex _thenRegex;
        private Regex _andRegex;

        private static readonly string _featurePattern = @"^\s*(?:{0})\s*:\s*(.+)";
        private static readonly string _scenarioPattern = @"^\s*(?:{0})\s*\d*\s*:\s*(.+)";
        private static readonly string _stepPattern = @"^\s*({0})\s+.+";
    }
}
