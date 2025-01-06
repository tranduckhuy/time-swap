using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Category>> GetCategoriesByIndustryAsync(int industryId)
        {
            return await _context.Categories
                .Where(category => category.IndustryId == industryId)
                .OrderBy(category => category.CategoryName)
                .ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoryIncludeIndustryAsync()
        {
            return await _context.Categories.Include(c => c.Industry).ToListAsync();
        }

    }
}
