using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class IndustryRepository : RepositoryBase<Industry, int>, IIndustryRepository
    {
        public IndustryRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<Industry?> GetByNameAsync(string industryName)
        {
            return await _context.Industries
                .FirstOrDefaultAsync(i => i.IndustryName.ToLower() == industryName.ToLower());
        }

    }
}
