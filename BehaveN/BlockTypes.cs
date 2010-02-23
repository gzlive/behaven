using System;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Helper methods for working with BlockType objects.
    /// </summary>
    public static class BlockTypes
    {
        static BlockTypes()
        {
            List<Type> types = new List<Type>(typeof(BlockTypes).Assembly.GetTypes());
            types = types.FindAll(delegate(Type t) { return typeof(BlockType).IsAssignableFrom(t) && !t.IsAbstract; });
            blockTypes = types.ConvertAll(delegate(Type t) { return (BlockType)Activator.CreateInstance(t); });
        }

        private static List<BlockType> blockTypes;

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
        /// <param name="type">The type.</param>
        /// <returns>The block type.</returns>
        public static BlockType GetBlockTypeFor(Type type)
        {
            if (type.IsByRef)
                type = type.GetElementType();

            return blockTypes.Find(delegate(BlockType it) { return it.HandlesType(type); });
        }

        /// <summary>
        /// Determines if a block type existis for the specific type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if a block type exists.</returns>
        public static bool BlockTypeExistsFor(Type type)
        {
            return GetBlockTypeFor(type) != null;
        }
    }
}
