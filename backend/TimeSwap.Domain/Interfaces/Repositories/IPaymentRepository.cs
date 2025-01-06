using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IAsyncRepository<Payment, Guid>
    {
        Task<Pagination<Payment>> GetPaymentsByUserIdAsync(Guid userId, string? paymentStatus, int dateFilter, int pageIndex = 1, int pageSize = 10);
    }
}
