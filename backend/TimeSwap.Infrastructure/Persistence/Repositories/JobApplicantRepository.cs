using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class JobApplicantRepository : RepositoryBase<JobApplicant, Guid>, IJobApplicantRepository
    {
        public JobApplicantRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<JobApplicant>> GetApplicantsByJobPostIdAsync(Guid jobPostId)
        {
            return await _context.JobApplicants
                .Where(x => x.JobPostId == jobPostId)
                .Include(x => x.UserApplied)
                .ToListAsync();
        }

        public async Task<int> GetTotalApplicantsByJobPostIdAsync(Guid jobPostId)
        {
            return await _context.JobApplicants.CountAsync(x => x.JobPostId == jobPostId);
        }

        public async Task<bool> IsUserAppliedToJobPostAsync(Guid jobPostId, Guid userId)
        {
            return await _context.JobApplicants.AnyAsync(x => x.JobPostId == jobPostId && x.UserAppliedId == userId);
        }
    }
}
