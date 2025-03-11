using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.User;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IAsyncRepository<UserProfile, Guid>
    {
        Task<UserProfile?> GetUserProfileAsync(Guid userId);
        Task<Pagination<UserProfile>?> GetUserWithSpecAsync(UserSpecParam param);
    }
}
