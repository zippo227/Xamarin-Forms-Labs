using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Serialization;

namespace Xamarin.Forms.Labs.Caching.Akavache
{
    public class AkavacheCache : IAsyncCacheProvider
    {
        private readonly IBlobCache cache;
        private readonly IByteSerializer serializer;

        public AkavacheCache(IBlobCache blobCache, IByteSerializer byteSerializer)
        {
            this.cache = blobCache;
            this.serializer = byteSerializer;
        }

        #region IAsyncCacheProvider Members

        public async Task<bool> AddAsync<T>(string key, T value, DateTime expiresAt)
        {
            var x = this.cache.Insert(key, this.serializer.SerializeToBytes(value), expiresAt);

            // TODO: find out why this isn't awaitable as advertized
            //var a = await x;

            throw new NotImplementedException();
        }

        public Task<bool> Set<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Replace<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Add<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Set<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAsyncSimpleCache Members

        public Task<bool> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReplaceAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task FlushAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task SetAllAsync<T>(IDictionary<string, T> values)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.cache.Dispose();
        }

        #endregion
    }
}
