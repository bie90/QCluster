using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace QCluster.Health
{
    /// <summary>
    /// IClusterHealthPolicy is the contract for determining the cluster health.
    /// </summary>
    public interface IClusterHealthPolicy
    {
        /// <summary>
        /// Determines the cluster health.
        /// </summary>
        HealthCheckResult CheckHealth();
    }
}