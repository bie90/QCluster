using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace QCluster.Health
{
    /// <summary>
    /// IClusterHealthPolicy is the contract for determining the cluster health.
    /// </summary>
    public interface IClusterHealthPolicy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workers"></param>
        /// <returns></returns>
        HealthCheckResult CheckHealth();
    }
}