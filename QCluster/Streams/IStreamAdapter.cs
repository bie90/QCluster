using System;

namespace QCluster.Streams
{
    /// <summary>
    /// IStreamAdapter is the adapter for a stream sending IStreamInstruction of type T.
    /// </summary>
    public interface IStreamAdapter
    {
        /// <summary>
        /// Publishes a instruction to the stream.
        /// </summary>
        /// <param name="instruction">Instruction</param>
        void Publish(IStreamInstruction instruction);
        /// <summary>
        /// Instruction provider.
        /// </summary>
        IStreamProvider Provider { get; set; }
    }
}
