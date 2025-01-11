using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IAsyncRepository<Payment, Guid>
    {
        Task<Pagination<Payment>> GetPaymentsByUserIdWithSpecAsync(PaymentByUserSpecParam param, Guid userId);
    }
}
