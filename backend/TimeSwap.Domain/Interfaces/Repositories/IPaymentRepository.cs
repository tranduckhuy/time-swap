using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IAsyncRepository<Payment, Guid>
    {
    }
}
