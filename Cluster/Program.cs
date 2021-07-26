using Microsoft.Extensions.Hosting;
using QCluster.Hosting;
namespace Cluster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Incomplete setup..
                    services.AddQCluster().UseMemoryStreams();
                });
    }
}
