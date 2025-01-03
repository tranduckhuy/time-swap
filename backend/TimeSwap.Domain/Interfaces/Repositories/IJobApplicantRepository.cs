using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IJobApplicantRepository : IAsyncRepository<JobApplicant, Guid>
    {
        Task<bool> IsUserAppliedToJobPostAsync(Guid jobPostId, Guid userId);
        Task<IEnumerable<JobApplicant>> GetApplicantsByJobPostIdAsync(Guid jobPostId);
    }
}
