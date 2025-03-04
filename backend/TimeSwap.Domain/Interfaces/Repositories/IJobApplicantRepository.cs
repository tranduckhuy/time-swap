using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IJobApplicantRepository : IAsyncRepository<JobApplicant, Guid>
    {
        Task<bool> IsUserAppliedToJobPostAsync(Guid jobPostId, Guid userId);
        Task<Pagination<JobApplicant>?> GetJobApplicantsAsync(JobApplicantSpecParam param);
        Task<int> GetTotalApplicantsByJobPostIdAsync(Guid jobPostId, CancellationToken cancellationToken);
        Task<bool> JobApplicantExistsAsync(Guid jobPostId, Guid userId, CancellationToken cancellationToken);
    }
}
