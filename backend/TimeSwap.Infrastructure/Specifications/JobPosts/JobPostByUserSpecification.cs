using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Infrastructure.Projections;

namespace TimeSwap.Infrastructure.Specifications.JobPosts
{
    public class JobPostByUserSpecification : ISpecification<JobPost>
    {
        public Expression<Func<JobPost, bool>> Criteria { get; private set; }
        public Func<IQueryable<JobPost>, IOrderedQueryable<JobPost>>? OrderBy { get; private set; }
        public Func<IQueryable<JobPost>, IOrderedQueryable<JobPost>>? OrderByDescending { get; private set; }
        public List<Expression<Func<JobPost, object>>> Includes { get; private set; } = [];
        public Func<IQueryable<JobPost>, IQueryable<JobPost>>? Selector { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public JobPostByUserSpecification(JobPostByUserSpecParam param)
        {
            // Filtering logic
            Criteria = x => x.UserId == param.UserId;

            // Sorting logic
            OrderBy = q => q.OrderByDescending(x => x.CreatedAt);

            // Includes
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Industry);

            // Pagination
            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;

            // Default selector to pick specific columns
            Selector = q => q.Select(JobPostProjections.SelectJobPostProjection());
        }
    }
}
