using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Infrastructure.Identity;

namespace TimeSwap.Infrastructure.Persistence.DbContexts
{
    public class UserIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        protected UserIdentityDbContext()
        {
        }

        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<MonthlyVisit> MonthlyVisits { get; set; }
    }
}
