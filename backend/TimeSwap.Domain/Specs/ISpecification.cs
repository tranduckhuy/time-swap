using System.Linq.Expressions;

namespace TimeSwap.Domain.Specs
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; }
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderByDescending { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        int Skip { get; }
        int Take { get; }
    }
}
