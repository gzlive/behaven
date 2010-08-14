// <copyright file="IBlock.cs" company="Jason Diamond">
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

    /// <summary>
    /// Represents a block parameter to a step definition.
    /// </summary>
    public interface IBlock
    {
        /// <summary>
        /// Converts the block into an object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <returns>The new object.</returns>
        object ConvertTo(Type type);

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns>A <c>string</c> representing the block.</returns>
        string Format();

        /// <summary>
        /// Checks that all of the values are on the specified object.
        /// </summary>
        /// <param name="actual">The object to check against.</param>
        /// <returns>True if the check succeeded.</returns>
        bool Check(object actual);

        /// <summary>
        /// Gets the suggested type for the parameter.
        /// </summary>
        /// <returns>The suggested type.</returns>
        string GetSuggestedParameterType();

        /// <summary>
        /// Gets the suggested name for the parameter.
        /// </summary>
        /// <returns>The suggested name.</returns>
        string GetSuggestedParameterName();

        /// <summary>
        /// Gets the suggested type definition for the parameter.
        /// </summary>
        /// <returns>The suggested type definition.</returns>
        string GetSuggestedParameterTypeDefinition();

        /// <summary>
        /// Reports to the reporter.
        /// </summary>
        /// <param name="reporter">The reporter.</param>
        void ReportTo(Reporter reporter);
    }
}
