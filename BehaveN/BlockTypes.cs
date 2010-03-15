// <copyright file="BlockTypes.cs" company="Jason Diamond">
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

    /// <summary>
    /// Helper methods for working with BlockType objects.
    /// </summary>
    public static class BlockTypes
    {
        /// <summary>
        /// The cached list of discovered block types.
        /// </summary>
        private static readonly List<BlockType> blockTypes;

        /// <summary>
        /// Initializes static members of the <see cref="BlockTypes"/> class.
        /// </summary>
        static BlockTypes()
        {
            List<Type> types = new List<Type>(typeof(BlockTypes).Assembly.GetTypes());
            types = types.FindAll(delegate(Type t) { return typeof(BlockType).IsAssignableFrom(t) && !t.IsAbstract; });
            blockTypes = types.ConvertAll(delegate(Type t) { return (BlockType)Activator.CreateInstance(t); });
        }

        /// <summary>
        /// Gets the block types.
        /// </summary>
        /// <returns>A list of BlockType objects.</returns>
        public static List<BlockType> GetBlockTypes()
        {
            return blockTypes;
        }

        /// <summary>
        /// Gets the block type that handles the specified type.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <returns>The block type.</returns>
        public static BlockType GetBlockTypeFor(Type type)
        {
            if (type.IsByRef)
            {
                type = type.GetElementType();
            }

            return blockTypes.Find(delegate(BlockType it) { return it.HandlesType(type); });
        }

        /// <summary>
        /// Determines if a block type existis for the specified value type.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <returns>True if a block type exists.</returns>
        public static bool BlockTypeExistsFor(Type type)
        {
            return GetBlockTypeFor(type) != null;
        }
    }
}
