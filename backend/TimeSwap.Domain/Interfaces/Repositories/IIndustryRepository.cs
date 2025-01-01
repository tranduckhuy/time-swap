using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IIndustryRepository : IAsyncRepository<Industry, int>
    {
        Task<Industry?> GetByNameAsync(string industryName);
    }
}
