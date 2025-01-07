using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Identity
{
    public class TransactionManager : ITransactionManager
    {
        private readonly UserIdentityDbContext _identityDbContext;
        private readonly AppDbContext _coreDbContext;

        public TransactionManager(UserIdentityDbContext identityDbContext, AppDbContext coreDbContext)
        {
            _identityDbContext = identityDbContext;
            _coreDbContext = coreDbContext;
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            using var identityTransaction = await _identityDbContext.Database.BeginTransactionAsync();
            using var coreTransaction = await _coreDbContext.Database.BeginTransactionAsync();

            try
            {
                await action(); // Thực hiện logic nghiệp vụ
                await identityTransaction.CommitAsync();
                await coreTransaction.CommitAsync();
            }
            catch
            {
                await identityTransaction.RollbackAsync();
                await coreTransaction.RollbackAsync();
                throw;
            }
        }
    }

}
