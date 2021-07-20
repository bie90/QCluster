using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace QCluster.Health
{
    /// <summary>
    /// ClusterHealth provides the default health check for the cluster.
    /// </summary>
    public class ClusterHealth : IHealthCheck
    {
        public ClusterHealth(IClusterHealthPolicy policy)
        {
            this.policy = policy;
        }

        #region IHealthCheck
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.policy.CheckHealth());
        }
        #endregion

        #region Private Members
        private IClusterHealthPolicy policy;
        #endregion
    }
}
