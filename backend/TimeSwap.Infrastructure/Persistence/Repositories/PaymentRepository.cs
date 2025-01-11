using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Specifications;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository(AppDbContext context) : RepositoryBase<Payment, Guid>(context), IPaymentRepository
    {
        public async Task<Pagination<Payment>> GetPaymentsByUserIdWithSpecAsync(PaymentByUserSpecParam param, Guid userId)
        {
            var spec = new PaymentByUserSpecification(param, userId);
            return await GetWithSpecAsync(spec);
        }
    }
}
