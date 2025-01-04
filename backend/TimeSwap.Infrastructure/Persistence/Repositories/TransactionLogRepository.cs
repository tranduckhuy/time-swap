using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class TransactionLogRepository : RepositoryBase<TransactionLog, Guid>, ITransactionLogRepository
    {
        public TransactionLogRepository(AppDbContext context) : base(context) { }

    }
}
