using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Specifications;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository(AppDbContext context) : RepositoryBase<Payment, Guid>(context), IPaymentRepository
    {
        public async Task<Pagination<Payment>> GetPaymentsByUserIdAsync(Guid userId, string? paymentStatus, int dateFilter, int pageIndex = 1, int pageSize = 10)
        {
            var spec = new PaymentByUserSpecification(userId, paymentStatus, dateFilter, pageIndex, pageSize);
            return await GetWithSpecAsync(spec);
        }
    }
}
