using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IIndustryRepository : IAsyncRepository<Industry, int>
    {
        Task<Industry?> GetIndustryByNameAsync(string industryName);
    }
}
