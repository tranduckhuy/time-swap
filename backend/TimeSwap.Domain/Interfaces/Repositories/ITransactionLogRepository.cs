using TimeSwap.Domain.Entities;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface ITransactionLogRepository : IAsyncRepository<TransactionLog, Guid>
    {
    }
}
