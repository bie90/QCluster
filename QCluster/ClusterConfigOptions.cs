namespace QCluster
{
    public class ClusterNodeOptions
    {
        /// <summary>
        /// The maximum number of tasks (instruction) being started at once.
        /// </summary>
        public int MaxTaskCount { get; set; } = 5;
    }
}
