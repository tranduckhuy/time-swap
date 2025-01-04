using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : RepositoryBase<Payment, Guid>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

    }
}
