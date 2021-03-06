// <copyright file="PlainTextReader.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
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

namespace BehaveN
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Reads specifications in the Given/When/Then style from a plain text file.
    /// </summary>
    public class PlainTextReader
    {
        private const string HeaderPattern = @"^\s*#\s*(.+)\s*:\s*(.+)\s*$";
        private const string CommentPattern = @"^\s*#.*$";
        private const string FeaturePattern = @"^\s*(?:{0})\s*:\s*(.+)";
        private const string ScenarioPattern = @"^\s*(?:{0})\s*\d*\s*:\s*(.*)";
        private const string StepPattern = @"^\s*({0})\s+.+";

        private readonly Regex headerRegex = new Regex(HeaderPattern);
        private readonly Regex commentRegex = new Regex(CommentPattern);
        private Regex featureRegex;
        private Regex scenarioRegex;
        private Regex givenRegex;
        private Regex whenRegex;
        private Regex thenRegex;
        private Regex andRegex;
        private Regex withRegex;

        private Match match;

        /// <summary>
        /// Reads the contents of the file to the feature.
        /// </summary>
        /// <param name="text">The text to read.</param>
        /// <param name="feature">The feature.</param>
        public void ReadTo(string text, Feature feature)
        {
            this.CompileRegexes(text);

            List<string> lines = TextParser.GetLines(text);

            Scenario scenario = null;
            StepType stepType = StepType.Unknown;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (scenario == null)
                {
                    this.match = this.headerRegex.Match(line);

                    if (this.match.Success)
                    {
                        feature.Headers[this.match.Groups[1].Value] = this.match.Groups[2].Value;

                        continue;
                    }

                    if (this.commentRegex.IsMatch(line))
                    {
                        continue;
                    }

                    i = this.ParseTitleAndDescription(lines, i, feature);
                }
                else
                {
                    if (this.commentRegex.IsMatch(line))
                    {
                        continue;
                    }

                    this.match = this.scenarioRegex.Match(line);
                }

                if (this.match.Success)
                {
                    scenario = CreateScenario(feature, this.match.Groups[1].Value, out stepType);
                }
                else
                {
                    if (scenario == null)
                    {
                        scenario = CreateScenario(feature, null, out stepType);
                    }

                    this.ParseStep(lines, ref i, scenario, ref stepType, line);
                }
            }
        }

        private Scenario CreateScenario(Feature feature, string name, out StepType stepType)
        {
            var scenario = new Scenario();
            scenario.Name = string.IsNullOrEmpty(name) ? "(no name)" : name;
            feature.Scenarios.Add(scenario);
            stepType = StepType.Unknown;
            return scenario;
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
            string with = lm.GetString(language, "With");

            this.featureRegex = new Regex(string.Format(FeaturePattern, feature), RegexOptions.IgnoreCase);
            this.scenarioRegex = new Regex(string.Format(ScenarioPattern, scenario), RegexOptions.IgnoreCase);
            this.givenRegex = new Regex(string.Format(StepPattern, given), RegexOptions.IgnoreCase);
            this.whenRegex = new Regex(string.Format(StepPattern, when), RegexOptions.IgnoreCase);
            this.thenRegex = new Regex(string.Format(StepPattern, then), RegexOptions.IgnoreCase);
            this.andRegex = new Regex(string.Format(StepPattern, and), RegexOptions.IgnoreCase);
            this.withRegex = new Regex(string.Format(StepPattern, with), RegexOptions.IgnoreCase);
        }

        private int ParseTitleAndDescription(List<string> lines, int i, Feature feature)
        {
            string line = lines[i];

            if ((this.match = this.featureRegex.Match(line)).Success) 
            {
                feature.Name = this.match.Groups[1].Value;
                i++;
            }

            var featureLines = new List<string>();

            for (; i < lines.Count; i++)
            {
                line = lines[i];

                this.match = this.scenarioRegex.Match(line);

                if (this.match.Success)
                {
                    break;
                }

                featureLines.Add(line);
            }

            feature.Description = string.Join("\r\n", featureLines.ToArray()).Trim('\r', '\n');

            return i;
        }

        private void ParseStep(List<string> lines, ref int i, Scenario scenario, ref StepType stepType, string line)
        {
            bool isPrimary = false;

            if (this.givenRegex.Match(line).Success)
            {
                stepType = StepType.Given;
                isPrimary = true;
            }
            else if (this.whenRegex.Match(line).Success)
            {
                stepType = StepType.When;
                isPrimary = true;
            }
            else if (this.thenRegex.Match(line).Success)
            {
                stepType = StepType.Then;
                isPrimary = true;
            }
            else if (this.andRegex.Match(line).Success)
            {
                if (stepType == StepType.Unknown)
                {
                    throw new Exception("\"And\" steps cannot appear before \"given\", \"when\", or \"then\" steps.");
                }
            }
            else if (this.withRegex.Match(line).Success)
            {
                if (stepType == StepType.Unknown)
                {
                    throw new Exception("\"With\" steps cannot appear before \"given\", \"when\", or \"then\" steps.");
                }
            }
            else
            {
                throw new Exception(string.Format("Unrecognized step: \"{0}\".", line));
            }

            IBlock block = this.ParseBlock(lines, ref i);

            scenario.Steps.Add(new Step
                               {
                                   Type = stepType,
                                   IsPrimary = isPrimary,
                                   Text = line,
                                   Block = block
                               });
        }

        private IBlock ParseBlock(List<string> lines, ref int i)
        {
            if (i + 1 >= lines.Count)
            {
                return null;
            }

            string line = lines[i + 1];

            var blockType = BlockTypes.GetBlockTypeForLine(line);

            if (blockType == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.AppendLine(line);

            i += 2;

            while (i < lines.Count && blockType.LineIsPartOfBlock(lines[i]))
            {
                sb.AppendLine(lines[i]);
                i++;
            }

            i--;

            IBlock block = blockType.Parse(sb.ToString());

            return block;
        }
    }
}
