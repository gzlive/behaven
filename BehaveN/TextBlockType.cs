// <copyright file="TextBlockType.cs" company="Jason Diamond">
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
    /// Block type for form values.
    /// </summary>
    internal class TextBlockType : BlockType
    {
        private static readonly Regex TextRegex = new Regex(@"^\s*> (.+)\s*");

        /// <summary>
        /// Determines if this type handles the specified type.
        /// </summary>
        /// <param name="type">The type that can potentially be handled by this block type.</param>
        /// <returns>
        /// true if this type handles the specified type
        /// </returns>
        public override bool HandlesType(Type type)
        {
            return type == typeof(StringBuilder);
        }

        /// <summary>
        /// Converts a block into a real object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <param name="block">The block to convert.</param>
        /// <returns>The real object.</returns>
        public override object GetObject(Type type, IBlock block)
        {
            return block.ConvertTo(type);
        }

        /// <summary>
        /// Determines if the lines the part of a block it handles.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>True or false.</returns>
        public override bool LineIsPartOfBlock(string line)
        {
            return TextRegex.IsMatch(line);
        }

        public override IBlock Parse(string text)
        {
            var sb = new StringBuilder();

            Match m;

            List<string> lines = TextParser.GetLines(text);
            int i = 0;

            while (i < lines.Count && (m = TextRegex.Match(lines[i])).Success)
            {
                if (i > 0)
                {
                    sb.AppendLine();
                }

                sb.Append(m.Groups[1].Value);

                i++;
            }

            return new Text(sb);
        }
    }
}