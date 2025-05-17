using Domain.Contracts;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        private readonly ICacheRepository _cacheRepository = cacheRepository;

        public async Task<string?> GetCacheValueAsync(string key)
        {
            var value = await _cacheRepository.GetAsync(key);
            return value == null ? null : value;
        }

        public async Task SetCacheValueAsync(string key, object value, TimeSpan duration)
        {
            await _cacheRepository.SetAsync(key, value, duration);
        }
    }
}
