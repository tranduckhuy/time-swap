using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Specifications;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class JobPostRepository : RepositoryBase<JobPost, Guid>, IJobPostRepository
    {
        public JobPostRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Pagination<JobPost>> GetJobPostsWithSpecAsync(JobPostSpecParam param)
        {
            var spec = new JobPostSpecification(param);
            return await GetWithSpecAsync(spec);
        }
    }
}
