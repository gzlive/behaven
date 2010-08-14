// <copyright file="Text.cs" company="Jason Diamond">
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
    using System.Text;

    /// <summary>
    /// Represents a form.
    /// </summary>
    public class Text : IBlock
    {
        private readonly StringBuilder stringBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        public Text(StringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;
        }

        /// <summary>
        /// Gets the string builder.
        /// </summary>
        /// <value>The string builder.</value>
        public StringBuilder StringBuilder
        {
            get { return this.stringBuilder; }
        }

        /// <summary>
        /// Converts the block into an object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <returns>The new object.</returns>
        public object ConvertTo(Type type)
        {
            if (type != typeof(StringBuilder))
            {
                throw new ArgumentException();
            }

            return this.stringBuilder;
        }

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns>A <c>string</c> representing the block.</returns>
        public string Format()
        {
            var lines = TextParser.GetLines(this.stringBuilder.ToString());

            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                sb.AppendLine("    > " + line);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks that all of the values are on the specified object.
        /// </summary>
        /// <param name="actual">The object to check against.</param>
        /// <returns></returns>
        public bool Check(object actual)
        {
            return this.stringBuilder.ToString() == ((StringBuilder)actual).ToString();
        }

        /// <summary>
        /// Gets the suggested type for the parameter.
        /// </summary>
        /// <returns>The type.</returns>
        public string GetSuggestedParameterType()
        {
            return "StringBuilder";
        }

        /// <summary>
        /// Gets the suggested name for the parameter.
        /// </summary>
        /// <returns>The name.</returns>
        public string GetSuggestedParameterName()
        {
            return "text";
        }

        /// <summary>
        /// Gets the suggested type definition for the parameter.
        /// </summary>
        /// <returns>The suggested type definition.</returns>
        public string GetSuggestedParameterTypeDefinition()
        {
            return null;
        }

        /// <summary>
        /// Reports to the reporter.
        /// </summary>
        /// <param name="reporter">The reporter.</param>
        public void ReportTo(Reporter reporter)
        {
            if (reporter is IBlockReporter<Text>)
            {
                ((IBlockReporter<Text>)reporter).ReportBlock(this);
            }
            else
            {
                throw new Exception(string.Format("{0} doesn't support reporting text.", reporter.GetType().FullName));
            }
        }
    }
}
