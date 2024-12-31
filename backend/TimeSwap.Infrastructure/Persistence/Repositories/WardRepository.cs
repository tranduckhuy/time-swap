using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class WardRepository : RepositoryBase<Ward, string>, IWardRepository
    {
        public WardRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ward>> GetWardsByCityIdAsync(string cityId)
        {
            return await _context.Wards
                .Where(w => w.District.CityId == cityId)
                .Include(w => w.District)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
