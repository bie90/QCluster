namespace QCluster.Streams.Memory
{
    public class MemoryStreamProvider : IStreamProvider
    {
        public IStreamInstruction Pop()
        {
            return MemoryCache.Pop();
        }
    }
}