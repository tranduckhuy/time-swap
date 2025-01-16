using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface ICityRepository : IAsyncRepository<City, string>
    {
        Task<IReadOnlyList<City>?> GetAllCitiesAsync();
    }
}
