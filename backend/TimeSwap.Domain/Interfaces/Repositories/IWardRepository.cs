using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IWardRepository : IAsyncRepository<Ward, string>
    {
        Task<IEnumerable<Ward>> GetWardsByCityIdAsync(string cityId);
    }
}
