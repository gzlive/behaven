using System;
using System.Collections;
using System.Collections.Generic;

namespace BehaveN
{
    /// <summary>
    /// Represents a generic object that can be converted to another object or a list of objects.
    /// </summary>
    public interface IConvertibleObject
    {
        /// <summary>
        /// Converts the convertible object into an object.
        /// </summary>
        /// <typeparam name="T">The type of object to convert to.</typeparam>
        /// <returns>The new object.</returns>
        T ToObject<T>();

        /// <summary>
        /// Converts the convertible object into an object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <returns>The new object.</returns>
        object ToObject(Type type);

        /// <summary>
        /// Converts the convertible object into a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to convert to.</typeparam>
        /// <returns>A list of objects.</returns>
        List<T> ToList<T>();

        /// <summary>
        /// Converts the convertible object into a list of objects.
        /// </summary>
        /// <param name="itemType">The type of objects to convert to.</param>
        /// <returns>A list of objects.</returns>
        IList ToList(Type itemType);

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns>A <c>string</c> representing the convertible object.</returns>
        string Format();

        /// <summary>
        /// Checks that all of the values are on the specified object.
        /// </summary>
        /// <param name="actual">The object to check against.</param>
        bool Check(object actual);
    }
}
