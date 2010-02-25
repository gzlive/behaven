using System;

namespace BehaveN
{
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
        bool Check(object actual);

        string GetSuggesterParameterType();

        string GetSuggesterParameterName();
    }
}
