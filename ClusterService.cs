using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using QCluster.Streams;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QCluster.Config;

namespace QCluster
{
    public class ClusterService : BackgroundService
    {
        public ClusterService(ILogger<ClusterService> logger, IServiceCollection services, IEnumerable<IStreamAdapter> adapters, IOptions<ClusterConfigOptions> options)
        {
            this.logger = logger;
            this.services = services;
            this.providers = adapters.Select(x => x.Provider);
            this.options = options.Value;
        }

        #region BackgroundService
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
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
            var listeners =  this.services.BuildServiceProvider().GetServices<IStreamListener<T>>();
            // Return a list of subscription tasks.
            var tasks = listeners.Select(x => x.OnSubscribeAsync(instruction)).ToList();
            return tasks;
        }
        #endregion
        #region  Private Members
        private IServiceCollection services;
        private IEnumerable<IStreamProvider> providers;
        private ClusterConfigOptions options;
        private ILogger<ClusterService> logger;
        private Guid instanceId;
        #endregion
    }
}