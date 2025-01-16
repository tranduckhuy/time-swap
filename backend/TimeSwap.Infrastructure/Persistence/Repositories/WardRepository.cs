using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class WardRepository : RepositoryBase<Ward, string>, IWardRepository
    {
        private readonly IDistributedCache _cache;

        public WardRepository(AppDbContext context, IDistributedCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<IReadOnlyList<Ward>?> GetWardsByCityIdAsync(string cityId)
        {
            var cacheKey = $"wards:{cityId}";

            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                return JsonSerializer.Deserialize<IReadOnlyList<Ward>>(cachedData);
            }

            var wards =  await _context.Wards
                .Where(w => w.District.CityId == cityId)
                .Include(w => w.District)
                .AsNoTracking()
                .Select(w => new Ward
                {
                    Id = w.Id,
                    Name = w.Name,
                    FullLocation = w.FullLocation
                })
                .ToListAsync();

            if (wards.Count > 0)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(wards), cacheOptions);
            }

            return wards;
        }

        public Task<bool> ValidateWardInCityAsync(string wardId, string cityId)
        {
            return _context.Wards
                .AnyAsync(w => w.Id == wardId && w.District.CityId == cityId);
        }
    }
}
