using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

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
    }
}
