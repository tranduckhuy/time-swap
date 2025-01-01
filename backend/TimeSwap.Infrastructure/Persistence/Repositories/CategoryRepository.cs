using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Specifications;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Pagination<Category>> GetCategoriesByIndustryAsync(int industryId, int pageIndex = 1, int pageSize = 10)
        {
            var spec = new CategoryByIndustrySpecification(industryId, pageIndex, pageSize);
            return await GetWithSpecAsync(spec);
        }

        public async Task<List<Category>> GetAllCategoryIncludeIndustryAsync()
        {
            return await _context.Categories.Include(c => c.Industry).ToListAsync();
        }

    }
}
