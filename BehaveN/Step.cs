// <copyright file="Step.cs" company="Jason Diamond">
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
    using System.Diagnostics;

    /// <summary>
    /// Represents an executable step in a scenario.
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class Step
    {
        /// <summary>
        /// Gets or sets the type of the step.
        /// </summary>
        public StepType Type { get; set; }

        /// <summary>
        /// Indicates if this step was specified using a "primary" keyword
        /// (given, when, or then).
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the actual text for the step.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the block value or null for the step.
        /// </summary>
        public IBlock Block { get; set; }

        /// <summary>
        /// Gets or sets the result of executing the step.
        /// </summary>
        public StepResult Result { get; set; }
   }
}