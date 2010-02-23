using System;

namespace BehaveN
{
    /// <summary>
    /// Represents an "undefined" step.
    /// </summary>
    public class UndefinedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndefinedException"/> class.
        /// </summary>
        public UndefinedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndefinedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UndefinedException(string message) : base(message)
        {
        }
    }
}
