namespace BehaveN
{
    /// <summary>
    /// Represents a reporter for blocks.
    /// </summary>
    /// <typeparam name="T">The block type.</typeparam>
    public interface IBlockReporter<T>
    {
        /// <summary>
        /// Reports the block.
        /// </summary>
        /// <param name="block">The block.</param>
        void ReportBlock(T block);
    }
}
