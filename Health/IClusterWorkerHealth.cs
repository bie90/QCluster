using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace QCluster.Health
{
    /// <summary>
    /// IClusterWorkerHealth is the contract for determining the health of cluster worker. 
    /// </summary>
    public interface IClusterWorkerHealth
    {
        /// <summary>
        /// The health of the worker.
        /// </summary>
        HealthCheckResult Health();
    }
}