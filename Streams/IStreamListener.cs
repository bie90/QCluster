using System.Threading.Tasks;

namespace QCluster.Streams
{
    /// <summary>
    /// IStreamListener is the contract for listening to instructions from the stream.
    /// </summary>
    public interface IStreamListener<T> where T : IStreamInstruction
    {
        /// <summary>
        /// Task to be carried out on subscription.
        /// </summary>
        /// <param name="instruction">Instruction</param>
        Task OnSubscribeAsync(T instruction);
    }
}