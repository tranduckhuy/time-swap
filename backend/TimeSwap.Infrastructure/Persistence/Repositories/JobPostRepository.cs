using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;
using System.Text.Json;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Projections;
using TimeSwap.Infrastructure.Specifications;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class JobPostRepository : RepositoryBase<JobPost, Guid>, IJobPostRepository
    {
        private readonly IDistributedCache _cache;

        public JobPostRepository(AppDbContext context, IDistributedCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<Pagination<JobPost>?> GetJobPostsWithSpecAsync(JobPostSpecParam param)
        {
            // Create cache key based on the spec
            var cacheKey = GenerateCacheKeyForSpec(param);
            var cachedData = await _cache.GetStringAsync(cacheKey);

            // If cache is found, return the cached data
            if (cachedData != null)
            {
                return JsonSerializer.Deserialize<Pagination<JobPost>>(cachedData);
            }

            var spec = new JobPostSpecification(param);
            var result = await GetWithSpecAsync(spec);

            // Cache the result
            if (result.Count > 0)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
            }

            return result;
        }

        private static string GenerateCacheKeyForSpec(JobPostSpecParam param)
        {
            return $"jobpost:spec:{param.Search}:{param.IndustryId}:{param.CategoryId}:{param.MinFee}:{param.MaxFee}:" +
                $"{param.PostedDate}:{param.CityId}:{param.WardId}:{param.Sort}:{param.PageIndex}:{param.PageSize}:{param.IsActive}";
        }

        public async Task<JobPost?> GetJobPostByIdAsync(Guid id)
        {
            return await _context.JobPosts
                .Include(x => x.Ward)
                .Include(x => x.Category)
                .Include(x => x.Industry)
                .Where(x => x.Id == id)
                .Select(JobPostProjections.SelectJobPostProjection())
                .FirstOrDefaultAsync();
        }

        public async Task<JobPost> CreateJobPostAsync(JobPost jobPost)
        {
            var result = await AddAsync(jobPost);

            await InvalidateCacheAsync();

            return result;
        }

        public async Task UpdateJobPostAsync(JobPost jobPost)
        {
            await UpdateAsync(jobPost);

            await InvalidateCacheAsync();
        }

        private async Task InvalidateCacheAsync()
        {
            await _cache.RemoveAsync("jobpost:spec:*");
        }

        public Task<int> GetUserJobPostCountOnCurrentDayAsync(Guid userId)
        {
            var today = DateTime.UtcNow.Date;
            return _context.JobPosts.CountAsync(x => x.UserId == userId && x.CreatedAt.Date == today);
        }

        public async Task<IEnumerable<JobPost>> GetRelatedJobPostsAsync(Guid jobPostId, int categoryId, int industryId, int limit)
        {
            return await _context.JobPosts
                .Include(x => x.Category)
                .Include(x => x.Industry)
                .Where(x => x.Id != jobPostId && (x.CategoryId == categoryId || x.IndustryId == industryId) && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .Select(JobPostProjections.SelectJobPostProjection())
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JobPost>> GetJobPostsByUserIdAsync(Expression<Func<JobPost, bool>> expression)
        {
            return await _context.JobPosts
                .Include(x => x.Category)
                .Include(x => x.Industry)
                .Where(expression)
                .OrderByDescending(x => x.CreatedAt)
                .Select(JobPostProjections.SelectJobPostProjection())
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
