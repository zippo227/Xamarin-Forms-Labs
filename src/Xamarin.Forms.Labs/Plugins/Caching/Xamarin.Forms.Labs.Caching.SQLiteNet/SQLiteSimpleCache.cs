using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Serialization;

namespace Xamarin.Forms.Labs.Caching.SQLiteNet
{
    public class SQLiteSimpleCache : SQLiteConnectionWithLock, ISimpleCache, IAsyncSimpleCache
    {
        private readonly IByteSerializer serializer;
        private readonly SQLiteAsyncConnection asyncConnection;

        public SQLiteSimpleCache(ISQLitePlatform platform, SQLiteConnectionString connection, IByteSerializer defaultSerializer)
            : base(platform, connection)
        {
            this.CreateTable<SQliteCacheTable>();
            this.serializer = defaultSerializer;
            this.asyncConnection = new SQLiteAsyncConnection(() => this);
        }

        #region ICacheProvider Members

        public bool Remove(string key)
        {
            return this.Delete<SQliteCacheTable>(key) == 1;
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            keys.Select(a => this.Remove(a));
        }

        public T Get<T>(string key)
        {
            return this.GetObject<T>(this.Find<SQliteCacheTable>(key));
        }

        public bool Add<T>(string key, T value)
        {
            return this.Insert(new SQliteCacheTable(key, this.GetBytes(value))) == 1;
        }

        public bool Set<T>(string key, T value)
        {
            var n = this.InsertOrReplace(new SQliteCacheTable(key, this.GetBytes(value)));
            return n == 1;
        }

        public bool Replace<T>(string key, T value)
        {
            return this.Remove(key) ?
                this.Add(key, value) :
                false;
        }

        public virtual void FlushAll()
        {
            this.DeleteAll<SQliteCacheTable>();
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            var dict = new Dictionary<string, T>();

            foreach (var item in keys.Select(a => new { Key = a, Item = this.Get<T>(a) }).Where(a => a.Item != null))
            {
                dict.Add(item.Key, item.Item);
            }

            return dict;
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            foreach (var value in values)
            {
                this.Set<T>(value.Key, value.Value);
            }
        }

        #endregion

        protected virtual T GetObject<T>(SQliteCacheTable item)
        {
            return (item != null) ? this.serializer.Deserialize<T>(item.Blob) : default(T);
        }

        protected virtual byte[] GetBytes<T>(T obj)
        {
            return this.serializer.SerializeToBytes(obj);
        }

        protected class SQliteCacheTable
        {
            public SQliteCacheTable()
            {
            }

            public SQliteCacheTable(string key, byte[] blob)
            {
                this.Key = key;
                this.Blob = blob;
            }

            [PrimaryKey]
            public string Key { get; set; }
            public byte[] Blob { get; set; }
        }

        #region IAsyncSimpleCache Members

        public async Task<bool> RemoveAsync(string key)
        {
            var count = await this.asyncConnection.DeleteAsync<SQliteCacheTable>(key);
            return count == 1;
        }

        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            await Task.WhenAll(keys.Select(key => this.RemoveAsync(key)));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var item = await this.asyncConnection.FindAsync<SQliteCacheTable>(key);

            return this.GetObject<T>(item);
        }

        public async Task<bool> AddAsync<T>(string key, T value)
        {
            var count = await this.asyncConnection.InsertAsync(new SQliteCacheTable(key, this.GetBytes(value)));
            return count == 1;
        }

        public async Task<bool> SetAsync<T>(string key, T value)
        {
            var n = await this.asyncConnection.InsertOrReplaceAsync(new SQliteCacheTable(key, this.GetBytes(value)));
            return n == 1;
        }

        public async Task<bool> ReplaceAsync<T>(string key, T value)
        {
            return await this.RemoveAsync(key) ? await this.AddAsync(key, value) : false;
        }

        public async Task FlushAllAsync()
        {
            await this.asyncConnection.DeleteAllAsync<SQliteCacheTable>();
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            var dict = new Dictionary<string, T>();

            foreach (var item in keys.Select(a => new { Key = a, Item = this.GetAsync<T>(a) }).Where(a => a.Item != null))
            {
                dict.Add(item.Key, await item.Item);
            }

            return dict;
        }

        public Task SetAllAsync<T>(IDictionary<string, T> values)
        {
            return Task.WhenAll(values.Select(value => this.SetAsync<T>(value.Key, value.Value)));
        }

        #endregion
    }
}
