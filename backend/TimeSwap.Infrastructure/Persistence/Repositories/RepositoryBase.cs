using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Infrastructure.Persistence.DbContexts;

namespace TimeSwap.Infrastructure.Persistence.Repositories
{
    public class RepositoryBase<TEntity, TKey> : IAsyncRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        protected readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pagination<TEntity>> GetWithSpecAsync<TSpec>(TSpec spec) where TSpec : ISpecification<TEntity>
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            foreach (var includeExpression in spec.Includes)
            {
                query = query.Include(includeExpression);
            }

            if (spec.OrderBy != null)
            {
                query = spec.OrderBy(query);
            }
                
            else if (spec.OrderByDescending != null)
            {
                query = spec.OrderByDescending(query);
            }

            var count = await query.CountAsync();

            var data = await query.Skip(spec.Skip).Take(spec.Take).ToListAsync();

            return new Pagination<TEntity>(spec.Skip / spec.Take + 1, spec.Take, count, data);
        }


        public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
    }
}
