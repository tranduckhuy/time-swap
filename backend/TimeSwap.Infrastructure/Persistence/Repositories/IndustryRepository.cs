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

        public async Task<Industry?> GetIndustryByNameAsync(string industryName)
        {
            return await _context.Industries
                .FirstOrDefaultAsync(i => EF.Functions.Unaccent(i.IndustryName).ToLower() == EF.Functions.Unaccent(industryName).ToLower());
        }
    }
}
