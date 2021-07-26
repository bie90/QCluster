using System;

namespace QCluster.Membership
{
    /// <summary>
    /// MembershipOptions provides options for the membership registry of the cluster.
    /// </summary>
    public class MembershipOptions
    {
        /// <summary>
        /// The interval which a node in the cluster should report membership.
        /// </summary>
        /// <value></value>
        public TimeSpan MemberInterval { get; set; }
    }
}