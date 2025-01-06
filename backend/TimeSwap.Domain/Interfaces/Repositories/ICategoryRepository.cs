using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IAsyncRepository<Category, int>
    {
        Task<List<Category>> GetCategoriesByIndustryAsync(int industryId);
        Task<List<Category>> GetAllCategoryIncludeIndustryAsync();
    }
}
