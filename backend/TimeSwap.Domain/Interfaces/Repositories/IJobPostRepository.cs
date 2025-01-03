using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IJobPostRepository : IAsyncRepository<JobPost, Guid>
    {
        Task<Pagination<JobPost>?> GetJobPostsWithSpecAsync(JobPostSpecParam param);
        Task<JobPost?> GetJobPostByIdAsync(Guid id);
        Task<JobPost> CreateJobPostAsync(JobPost jobPost);
        Task UpdateJobPostAsync(JobPost jobPost);
        Task<int> GetUserJobPostCountOnCurrentDayAsync(Guid userId);
    }
}
