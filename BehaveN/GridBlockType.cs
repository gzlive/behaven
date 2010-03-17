// <copyright file="GridBlockType.cs" company="Jason Diamond">
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

using System.Collections.Generic;

namespace BehaveN
{
    using System;
    using System.Text.RegularExpressions;

    internal class GridBlockType : BlockType
    {
        private static readonly Regex GridRegex = new Regex(@"^\s*\|", RegexOptions.IgnoreCase);

        public override bool HandlesType(Type type)
        {
            return type != typeof(string) && TypeExtensions.GetCollectionItemType(type) != null;
        }

        public override object GetObject(Type type, IBlock block)
        {
            return block.ConvertTo(type);
        }

        public override bool LineIsPartOfBlock(string line)
        {
            return GridRegex.IsMatch(line);
        }

        public override IBlock Parse(string text)
        {
            var grid = new Grid();

            List<string> lines = TextParser.GetLines(text);
            int i = 0;

            List<string> headers = SplitCells(lines[i]);
            grid.SetHeaders(headers);

            i++;

            while (i < lines.Count && GridRegex.IsMatch(lines[i]))
            {
                grid.AddValues(SplitCells(lines[i]));
                i++;
            }

            return grid;
        }

        /// <summary>
        /// Splits the cells in a line.
        /// </summary>
        /// <param name="line">The line of text to split.</param>
        /// <returns>A list of values in the cells.</returns>
        private static List<string> SplitCells(string line)
        {
            line = line.Trim();
            line = line.Trim('|');

            List<string> cells = new List<string>();

            foreach (string cell in line.Split('|'))
            {
                cells.Add(cell.Trim());
            }

            return cells;
        }
    }
}