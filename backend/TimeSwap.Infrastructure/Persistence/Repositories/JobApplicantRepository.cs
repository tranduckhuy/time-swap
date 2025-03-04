using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Specifications;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class JobApplicantRepository : RepositoryBase<JobApplicant, Guid>, IJobApplicantRepository
    {
        public JobApplicantRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Pagination<JobApplicant>?> GetJobApplicantsAsync(JobApplicantSpecParam param)
        {
            var spec = new JobApplicantSpecification(param);
            return await GetWithSpecAsync(spec);
        }

        public async Task<int> GetTotalApplicantsByJobPostIdAsync(Guid jobPostId)
        {
            return await _context.JobApplicants.CountAsync(x => x.JobPostId == jobPostId);
        }

        public async Task<bool> IsUserAppliedToJobPostAsync(Guid jobPostId, Guid userId)
        {
            return await _context.JobApplicants.AnyAsync(x => x.JobPostId == jobPostId && x.UserAppliedId == userId);
        }

        public Task<bool> JobApplicantExistsAsync(Guid jobPostId, Guid userId)
        {
            return _context.JobApplicants.AnyAsync(x => x.JobPostId == jobPostId && x.UserAppliedId == userId);
        }
    }
}
