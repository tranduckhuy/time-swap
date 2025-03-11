using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.User;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Specifications.JobPosts;
using TimeSwap.Infrastructure.Specifications.User;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<UserProfile, Guid>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserProfile?> GetUserProfileAsync(Guid userId)
        {
            return await _context.UserProfiles
                .Include(u => u.MajorCategory)
                .Include(u => u.MajorIndustry)
                .Include(u => u.Ward)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Pagination<UserProfile>?> GetUserWithSpecAsync(UserSpecParam param)
        {
            var spec = new UserSpecification(param);
            return await GetWithSpecAsync(spec);
        }
    }
}
