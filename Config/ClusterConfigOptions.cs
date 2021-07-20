using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace QCluster.Config
{
    public class ClusterConfigOptions
    {
        /// <summary>
        /// The maximum number of tasks (instruction) being started at once.
        /// </summary>
        /// <value></value>
        public int MaxTaskCount { get; set; } = 5;
    }
}
