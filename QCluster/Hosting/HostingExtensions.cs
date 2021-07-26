using Microsoft.Extensions.DependencyInjection;
using QCluster.Nodes;
using QCluster.Streams;
using QCluster.Streams.Memory;

namespace QCluster.Hosting
{
    public static class HostingExtensions
    {
        /// <summary>
        /// Adds QCluster to a serviceCollection.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static IServiceCollection AddQCluster(this IServiceCollection services)
        {
            services.AddHostedService<ClusterNode>();
            return services;
        }

        /// <summary>
        /// Adds MemoryStreamAdapter as a adapter.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static IServiceCollection UseMemoryStreams(this IServiceCollection services)
        {
            services.AddSingleton<IStreamAdapter, MemoryStreamAdapter>();
            return services;
        }
    }
}