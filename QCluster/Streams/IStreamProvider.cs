using System;

namespace QCluster.Streams
{
    /// <summary>
    /// IStreamProvider is contract for the provider of instructions allowing for integration with external providers.
    /// </summary>
    public interface IStreamProvider
    {
        /// <summary>
        /// Attempts to pop a message from a IStreamProvider
        /// </summary>
        IStreamInstruction Pop();
    }
}
