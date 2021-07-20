using System;
using System.Threading.Tasks;

namespace QCluster.Streams
{
    /// <summary>
    /// IStreamInstruction is the contract for instructions to be carried out.
    /// </summary>
    public interface IStreamInstruction
    {
        /// <summary>
        /// Instruction id.
        /// </summary>
        /// <value></value>
        Guid InstrutionId { get; set ;}
        /// <summary>
        /// The payload, this represents the initial state of the instruction.
        /// </summary>
        byte[] Payload { get; set; }
        /// <summary>
        /// Instuct.
        /// </summary>
        Task Instruct();
    }
}