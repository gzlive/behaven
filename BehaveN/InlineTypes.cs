using System;
using System.Collections.Generic;
using System.Text;

namespace BehaveN
{
    /// <summary>
    /// Helper methods for working with InlineType objects.
    /// </summary>
    public static class InlineTypes
    {
        static InlineTypes()
        {
            List<Type> types = new List<Type>(typeof(InlineTypes).Assembly.GetTypes());
            types = types.FindAll(delegate(Type t) { return typeof(InlineType).IsAssignableFrom(t) && !t.IsAbstract; });
            inlineTypes = types.ConvertAll(delegate(Type t) { return (InlineType)Activator.CreateInstance(t); });
        }

        private static List<InlineType> inlineTypes;

        /// <summary>
        /// Gets the inline types.
        /// </summary>
        /// <returns>A list of InlineType objects.</returns>
        public static List<InlineType> GetInlineTypes()
        {
            return inlineTypes;
        }

        /// <summary>
        /// Gets the inline type that handles the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The inline type.</returns>
        public static InlineType GetInlineTypeFor(Type type)
        {
            if (type.IsByRef)
                type = type.GetElementType();

            return inlineTypes.Find(delegate(InlineType it) { return it.HandlesType(type); });
        }

        /// <summary>
        /// Determines if an inline type existis for the specific type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if an inline type exists.</returns>
        public static bool InlineTypeExistsFor(Type type)
        {
            return GetInlineTypeFor(type) != null;
        }
    }
}
