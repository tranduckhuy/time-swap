using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IWardRepository : IAsyncRepository<Ward, string>
    {
        Task<IReadOnlyList<Ward>?> GetWardsByCityIdAsync(string cityId);

        Task<bool> ValidateWardInCityAsync(string wardId, string cityId);
    }
}
