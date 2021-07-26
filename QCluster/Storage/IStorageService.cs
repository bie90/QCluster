using System;
using System.Threading;
using System.Threading.Tasks;

namespace QCluster.Storage
{
    /// <summary>
    /// IStorageService is the contract for storage services used by nodes within the cluster.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Registers a object of type T in the storage.
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <param name="expiration">Expiration timeoff if supported by implementation.</param>
        Task RegisterAsync<T>(T o, CancellationToken cancellationToken, TimeSpan expiration) where T : IStorageObject;
        /// <summary>
        /// Returns the a object of type T from the storge by key.
        /// </summary>
        /// <param name="key">Storage key.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken) where T : IStorageObject;
        /// <summary>
        /// Determines whether a record exists for given key.
        /// </summary>
        /// <param name="key">Storage key.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        Task<bool> ExistsAsync(string key, CancellationToken cancellationToken);
    }
}