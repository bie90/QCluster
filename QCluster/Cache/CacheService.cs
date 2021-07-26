using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using QCluster.Storage;

namespace QCluster.Cache
{
    /// <summary>
    /// CacheService is a abstraction for type T and IDistributedCache.
    /// </summary>
    public class CacheService : IStorageService
    {
        /// <summary>
        /// CacheService cstor.
        /// </summary>
        /// <param name="cache">Cache implementation.</param>
        public CacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        #region Public API
        public async Task RegisterAsync<T>(T o, CancellationToken cancellationToken, TimeSpan expiration) where T : IStorageObject
        {
            await this.cache.SetAsync(
                o.Key,
                this.ToByteArray(o),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                },
                cancellationToken
            );
        }

        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken)
        {
            var res = await this.cache.GetAsync(key, cancellationToken);
            return res == null;
        }
        
        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken) where T : IStorageObject
        {
            var res = await this.cache.GetAsync(key, cancellationToken);
            if(res == null)
            {
                throw new ArgumentException($"There is no member with id: '{key}'");
            }
            return (T)ToObject(res);
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