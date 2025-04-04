using Microsoft.Extensions.Caching.Memory;

namespace Swizom.Utility
{
    public static class CacheHelper
    {
        public static async Task<(T result, bool fromCache)> GetOrSetAsync<T>(
        IMemoryCache cache,
        string key,
        Func<Task<T>> fetchData,
        int absoluteExpirationMinutes = 5,
        int slidingExpirationMinutes = 2)
        {
            if (cache.TryGetValue(key, out T cachedData))
            {
                return (cachedData, true); // From cache
            }

            var data = await fetchData();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationMinutes)
            };

            cache.Set(key, data, cacheOptions);

            return (data, false); // Fresh fetch
        }
    }
}
