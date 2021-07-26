namespace QCluster.Streams.Memory
{
    public class MemoryStreamAdapter : IStreamAdapter
    {
        public MemoryStreamAdapter()
        {
            Provider = new MemoryStreamProvider();
        }
        public IStreamProvider Provider { get; set; }

        public void Publish(IStreamInstruction instruction)
        {
            MemoryCache.Publish(instruction);
        }
        MemoryStreamAdapter adapter;
    }
}