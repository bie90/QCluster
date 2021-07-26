using System;

namespace QCluster.Telemetrics
{
    /// <summary>
    /// IClusterNodeTelemetry is the contract for the telemetry of a node within the cluster.
    /// </summary>
    public interface IClusterNodeTelemetry
    {
         // hook telemetrics, allow for controller in third party to add controllers.
         /// <summary>
         /// InstanceId, represents the telemetry being tracked.
         /// </summary>
         Guid InstanceId { get; set; }
         /// <summary>
         /// Telemetric payload, interpreted from InstanceId
         /// </summary>
         object Payload { get; set; }         
    }
}