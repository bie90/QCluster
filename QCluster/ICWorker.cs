using System.Threading.Tasks;
using QCluster.Streams;
namespace QCluster
{
     /// <summary>
    /// IQCWorker is the contract for a instruction based queue worker working witin a cluster.
    /// </summary>
    public interface IQCWorker<T> where T : IStreamInstruction
    {
        /// <summary>
        /// Executed on worker initiation.
        /// </summary>
        Task InitAsync();
        /// <summary>
        /// Carry out instruction.
        /// </summary>
        /// <param name="instruction">Instruction.</param>
        Task Instruct(T instruction);
    }
}