using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IAsyncRepository<UserProfile, Guid>
    {
        Task<UserProfile?> GetUserProfileAsync(Guid userId);
    }
}
