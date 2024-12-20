using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Repositories
{
    public interface IUserRepository : IAsyncRepository<UserProfile>
    {
    }
}
