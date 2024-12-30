using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IJobPostRepository : IAsyncRepository<JobPost, Guid>
    {
        Task<Pagination<JobPost>> GetJobPostsWithSpecAsync(JobPostSpecParam param);
    }
}
