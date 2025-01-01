using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Infrastructure.Specifications
{
    public class CategoryByIndustrySpecification : ISpecification<Category>
    {
        public Expression<Func<Category, bool>> Criteria { get; private set; }
        public Func<IQueryable<Category>, IOrderedQueryable<Category>>? OrderBy { get; private set; }
        public Func<IQueryable<Category>, IOrderedQueryable<Category>>? OrderByDescending { get; private set; }
        public List<Expression<Func<Category, object>>> Includes { get; private set; } = new List<Expression<Func<Category, object>>>();
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public CategoryByIndustrySpecification(int industryId, int pageIndex = 1, int pageSize = 10)
        {
            Criteria = category => category.IndustryId == industryId;

            OrderBy = q => q.OrderBy(x => x.CategoryName);

            Includes.Add(x => x.Industry);

            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
        }
    }
}
