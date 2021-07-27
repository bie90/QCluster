using Microsoft.Extensions.Hosting;
using QCluster.Hosting;
namespace Cluster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => 
                {
                    services.AddQCluster()
                            .UseMemoryStreams();
                })
                .Build()
                .Run();
        }
    }
}
