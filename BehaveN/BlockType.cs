using System;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a block parameter type.
    /// </summary>
    public abstract class BlockType
    {
        /// <summary>
        /// Determines if this type handles the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true if this type handles the specified type</returns>
        public abstract bool HandlesType(Type type);

        /// <summary>
        /// Converts a convertible object into a real object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <param name="convertibleObject">The convertible object to convert.</param>
        /// <returns>The real object.</returns>
        public abstract object GetObject(Type type, IConvertibleObject convertibleObject);

        internal static Type GetCollectionItemType(Type type)
        {
            if (!type.IsGenericType) return null;

            Type genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(IEnumerable<>) ||
                genericType == typeof(IList<>) ||
                genericType == typeof(ICollection<>) ||
                genericType == typeof(List<>))
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }
    }
}
