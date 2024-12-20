using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;

namespace TimeSwap.Infrastructure.Data
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
        {
        }
    }
}
