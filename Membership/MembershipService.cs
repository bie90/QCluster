using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

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
        /// <param name="cache">Cache used as backplane.</param>
        public MembershipService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        #region Public API
        /// <summary>
        /// Registers the fact that a node is still part of the cluster.
        /// </summary>
        /// <param name="checkin">MembershipCheckin</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task RegisterAsync(MembershipCheckin checkin, CancellationToken cancellationToken)
        {
            await this.cache.SetAsync(
                checkin.InstanceId.ToString(),
                ToByteArray(checkin),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = new TimeSpan(0, 5, 0)
                },
            cancellationToken);
        }

        /// <summary>
        /// Returns the latest checkin from a node within the cluster.
        /// </summary>
        /// <param name="instanceId">Id of node instance.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task<MembershipCheckin> GetAsync(Guid instanceId, CancellationToken cancellationToken)
        {
            var res = await this.cache.GetAsync(instanceId.ToString(), cancellationToken);
            if(res == null)
            {
                throw new ArgumentException($"There is no member with id: '{instanceId.ToString()}'");
            }
            return (MembershipCheckin)ToObject(res);
        }

        /// <summary>
        /// Determines whether a node is still a member of the cluster.
        /// </summary>
        /// <param name="instanceId">Id of node instance.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task<bool> IsMember(Guid instanceId, CancellationToken cancellationToken)
        {
            var res = await this.cache.GetAsync(instanceId.ToString(), cancellationToken);
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
        #endregion

        #region Private Members
        private readonly IDistributedCache cache;
        #endregion
    }
}