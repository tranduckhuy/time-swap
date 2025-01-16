using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class CityRepository : RepositoryBase<City, string>, ICityRepository
    {
        private readonly IDistributedCache _cache;

        public CityRepository(AppDbContext context, IDistributedCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<IReadOnlyList<City>?> GetAllCitiesAsync()
        {
            var cacheKey = "cities";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                return JsonSerializer.Deserialize<IReadOnlyList<City>>(cachedData);
            }

            var cities = await GetAllAsync();

            if (cities.Count > 0)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(cities), cacheOptions);
            }

            return cities;
        }
    }
}
