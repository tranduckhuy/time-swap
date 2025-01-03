using Microsoft.Extensions.Caching.Distributed;
using TimeSwap.Application.Authentication.Interfaces;

namespace TimeSwap.Infrastructure.Authentication
{
    public class TokenBlackListService : ITokenBlackListService
    {
        private readonly IDistributedCache _cache;

        public TokenBlackListService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task BlacklistTokenAsync(string token, DateTime expiry)
        {
            var key = $"blacklist:{token}";
            var ttl = (expiry - DateTime.UtcNow).TotalSeconds;

            if (ttl > 0)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttl)
                };

                // Save token to cache with time to live
                await _cache.SetStringAsync(key, "blacklisted", options);
            }
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            var key = $"blacklist:{token}";
            var result = await _cache.GetStringAsync(key);
            return result != null;
        }
    }
}
