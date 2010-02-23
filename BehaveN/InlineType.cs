using System;

namespace BehaveN
{
    /// <summary>
    /// Represents an inline parameter type.
    /// </summary>
    public abstract class InlineType
    {
        /// <summary>
        /// Determines if this type handles the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>true if this type handles the specified type</returns>
        public abstract bool HandlesType(Type type);

        /// <summary>
        /// Gets the pattern for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The pattern.</returns>
        public abstract string GetPattern(Type type);
    }
}
