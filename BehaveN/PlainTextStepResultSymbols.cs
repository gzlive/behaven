namespace BehaveN
{
    /// <summary>
    /// String constants used by the plain text reportes for indicating step results.
    /// </summary>
    public static class PlainTextStepResultSymbols
    {
        /// <summary>
        /// The symbol for an undefined step.
        /// </summary>
        public const string Undefined = "?";

        /// <summary>
        /// The symbol for a pending step.
        /// </summary>
        public const string Pending = "*";

        /// <summary>
        /// The symbol for a passed step.
        /// </summary>
        public const string Passed = " ";

        /// <summary>
        /// The symbol for a failed step.
        /// </summary>
        public const string Failed = "!";

        /// <summary>
        /// The symbol for a skipped step.
        /// </summary>
        public const string Skipped = "-";
    }
}
