using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Repositories;
using TimeSwap.Infrastructure.Data;

namespace TimeSwap.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<UserProfile>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
