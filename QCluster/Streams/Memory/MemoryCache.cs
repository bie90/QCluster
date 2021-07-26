using System.Collections.Generic;

namespace QCluster.Streams.Memory
{
    /// <summary>
    /// MemoryCache is a static memory cache, suitable for local/deevelopment enviroments.
    /// It acts as a threadsafe provider and adapter.
    /// </summary>
    internal class MemoryCache
    {
        private static List<IStreamInstruction> Cache { get; set; }

        public static IStreamInstruction Pop()
        {
            // Check if empty
            if(Cache.Count == 0) return null;
            lock(lockobject)
            {
                // Check if empty after locking.
                if(Cache.Count == 0) return null;
                var obj = Cache[0];
                Cache.RemoveAt(0);
                return obj;
            }
        }

        public static void Publish(IStreamInstruction instruction)
        {
            lock(lockobject)
            {
                Cache.Insert(Cache.Count, instruction);
            }
        }
        private  static object lockobject;
        private IStreamProvider streamProvider;
    }
}