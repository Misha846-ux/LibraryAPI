using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Books.Application.Interfaces.Services;
using Books.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Books.Infrastructure.Services
{
    public class RedisCachingService : ICachingServices
    {
        private readonly IDatabase _dataBase;
        private readonly CachingSettings _cachingSettings;
        public RedisCachingService(IConnectionMultiplexer multiplexer, IOptions<CachingSettings> options)
        {
            this._dataBase = multiplexer.GetDatabase();
            this._cachingSettings = options.Value;
        }
        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            var value = await _dataBase.StringGetAsync(key);
            if(value.IsNullOrEmpty)
            {
                return default(T?);
            }
            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            await _dataBase.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? exp, CancellationToken cancellationToken)
        {
            string json = JsonSerializer.Serialize(value);
            if(exp != null)
            {
                await _dataBase.StringSetAsync(key, json, exp.Value);
            }
            else
            {
                await _dataBase.StringSetAsync(key, json, _cachingSettings.LifeTime);
            }
        }
    }
}
