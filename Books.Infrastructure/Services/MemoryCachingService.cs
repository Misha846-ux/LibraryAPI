using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Application.Interfaces.Services;
using Books.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Books.Application.Service
{
    public class MemoryCachingService : ICachingServices
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CachingSettings _cachingSettings;
        public MemoryCachingService(IMemoryCache memoryCache, IOptions<CachingSettings> options)
        {
            this._memoryCache = memoryCache;
            this._cachingSettings = options.Value;
        }

        public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            if(_memoryCache.TryGetValue(key, out var value))
            {
                return Task.FromResult((T)value);
            }
            return Task.FromResult((T)default);
            
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? exp, CancellationToken cancellationToken)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = exp ?? _cachingSettings.LifeTime
            };
            _memoryCache.Set(key, value, options);
            return Task.CompletedTask;
        }
    }
}
