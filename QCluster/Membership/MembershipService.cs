using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using QCluster.Cache;
using QCluster.Models;
using QCluster.Storage;

namespace QCluster.Membership
{
    /// <summary>
    /// MembershipService provides the backplane for the nodes in a QCluster.
    /// </summary>
    public class MembershipService
    {
        /// <summary>
        /// MembershipService cstor.
        /// </summary>
        /// <param name="storageService">Storage used as backplane.</param>
        /// <param name="options">Options to be used.</param>
        public MembershipService(IStorageService storageService, MembershipOptions options)
        {
            this.storageService = storageService;
            this.options = options;
        }

        #region Public API
        /// <summary>
        /// Registers the fact that a node is still part of the cluster.
        /// </summary>
        /// <param name="instanceId">Id of node instance.</param>
        /// <param name="aliveness">Aliveness</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task RegisterAsync(Guid instanceId, Aliveness aliveness, CancellationToken cancellationToken, TimeSpan expiration)
        {
            await this.storageService.RegisterAsync(
                aliveness,
                cancellationToken,
                expiration
            );
        }

        /// <summary>
        /// Returns the latest checkin from a node within the cluster.
        /// </summary>
        /// <param name="instanceId">Id of node instance.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task<Aliveness> GetAsync<T>(Guid instanceId, CancellationToken cancellationToken)
        {
            var res = await this.storageService.GetAsync<Aliveness>(this.CacheKey(instanceId), cancellationToken);
            if(res == null)
            {
                throw new ArgumentException($"There is no member with id: '{instanceId.ToString()}'");
            }
            return res;
        }

        /// <summary>
        /// Determines whether a node is still a member of the cluster.
        /// </summary>
        /// <param name="instanceId">Id of node instance.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task<bool> IsMember(Guid instanceId, CancellationToken cancellationToken)
        {
            var res = await this.storageService.GetAsync<Aliveness>(this.CacheKey(instanceId), cancellationToken);
            return res == null;
        }
        #endregion

        #region Private Helpers
        /// <summary>
        /// Converts a byte array to object.
        /// </summary>
        /// <param name="arrBytes">Array of bytes</param>

        private object ToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        /// <summary>
        /// Converts a object to a byte array.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        private byte[] ToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private string CacheKey(Guid instanceId)
        {
            return $"memberhip-member-{instanceId.ToString()}";
        }
        #endregion

        #region Private Members
        private MembershipOptions options;
        private readonly IStorageService storageService;
        #endregion
    }
}