using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using QCluster.Storage;

namespace QCluster.Models
{
    public class Aliveness : IStorageObject
    {
        public string Key =>  $"aliveness-{this.Id}";
        public Guid Id { get; set; }
        public HealthCheckResult Health { get; set; }
    }
}