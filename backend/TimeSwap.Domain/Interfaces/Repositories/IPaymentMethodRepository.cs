using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IPaymentMethodRepository : IAsyncRepository<PaymentMethod, int>
    {
    }
}
