using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IAsyncRepository<Category, int>
    {
        Task<Pagination<Category>> GetCategoriesByIndustryAsync(int industryId, int pageIndex = 1, int pageSize = 10);
        Task<List<Category>> GetAllCategoryIncludeIndustryAsync();
    }
}
