using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using QCluster.Streams;
using Microsoft.Extensions.DependencyInjection;
using QCluster.Membership;
using QCluster.Health;
using QCluster.Storage;
using Microsoft.Extensions.Options;

namespace QCluster.Nodes
{
    /// <summary>
    /// ClusterNode reprsents a single node within a QCluster and is the service root for the instance.
    /// </summary>
    public class ClusterNode : BackgroundService
    {
        public ClusterNode(
            ILogger<ClusterNode> logger,
            IServiceCollection services,
            IEnumerable<IStreamAdapter> adapters,
            IOptions<ClusterNodeOptions> clusterOptions,
            IOptions<MembershipOptions> membershipOptions,
            IStorageService storage,
            IClusterHealthPolicy healthPolicy)
        {
            this.logger = logger;
            this.services = services;
            this.providers = adapters.Select(x => x.Provider);
            this.clusterOptions = clusterOptions is null ? new ClusterNodeOptions() : clusterOptions.Value;
            this.membershipOptions = membershipOptions is null ? new MembershipOptions() : membershipOptions.Value;
            this.instanceId = Guid.NewGuid();
            this.storage = storage;
            this.healthPolicy = healthPolicy;
            this.membershipService = new MembershipService(this.storage, this.membershipOptions);

        }

        #region BackgroundService
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Register Node with cluster.
            return this.InitAdapters(cancellationToken);
        }
        #endregion

        #region Private Helpers
        private Task InitAdapters(CancellationToken cancellationToken)
        {
            // TODO: Look over task management..
            Parallel.ForEach(this.providers, provider => {
                while(!cancellationToken.IsCancellationRequested)
                {
                    var instruction = provider.Pop();
                    if(instruction != null)
                    {
                        // If instruction, put blocking call to catch tasks.
                        Task.WhenAll(this.Notify(instruction));
                    }
                }
            });
            return Task.CompletedTask;
        }

        private List<Task> Notify<T>(T instruction) where T : IStreamInstruction
        {
            // Get listeners based on Type from runtime.
            var notifications = this.GetType()
                                .GetMethod("NotifyListeners")
                                .MakeGenericMethod(typeof(T))
                                .Invoke(this, new object[] { instruction });
            // Store tasks for notification
            return (List<Task>)notifications;            
        }

        private List<Task> NotifyListeners<T>(T instruction) where T : IStreamInstruction
        {
            // Get listeners based on type T.
            var listeners =  this.services.BuildServiceProvider()
                                          .GetServices<IStreamListener<T>>();
            // Return a list of subscription tasks.
            var tasks = listeners.Select(x => x.OnSubscribeAsync(instruction)).ToList();
            return tasks;
        }
        #endregion
        #region  Private Members
        private IServiceCollection services;
        private MembershipService membershipService;
        private IEnumerable<IStreamProvider> providers;
        private IEnumerable<IStreamListener<IStreamInstruction>> listeners;
        private ClusterNodeOptions clusterOptions;
        private MembershipOptions membershipOptions;
        private ILogger<ClusterNode> logger;
        private IStorageService storage;
        private IClusterHealthPolicy healthPolicy;
        private Guid instanceId;
        #endregion
    }
}