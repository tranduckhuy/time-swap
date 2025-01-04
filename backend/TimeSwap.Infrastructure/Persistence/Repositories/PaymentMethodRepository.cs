using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class PaymentMethodRepository : RepositoryBase<PaymentMethod, int>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(AppDbContext context) : base(context) { }

    }
}
