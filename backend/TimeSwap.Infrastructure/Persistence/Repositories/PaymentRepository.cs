using Microsoft.EntityFrameworkCore;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : RepositoryBase<Payment, Guid>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Include(p => p.PaymentMethod)
                .Include(p => p.TransactionLogs)
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
