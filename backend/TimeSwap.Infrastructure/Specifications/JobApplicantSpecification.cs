using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Infrastructure.Specifications
{
    public class JobApplicantSpecification : ISpecification<JobApplicant>
    {
        public Expression<Func<JobApplicant, bool>> Criteria { get; private set; }

        public Func<IQueryable<JobApplicant>, IOrderedQueryable<JobApplicant>>? OrderBy { get; private set; }

        public Func<IQueryable<JobApplicant>, IOrderedQueryable<JobApplicant>>? OrderByDescending { get; private set; }

        public List<Expression<Func<JobApplicant, object>>> Includes { get; private set; } = [];

        public Func<IQueryable<JobApplicant>, IQueryable<JobApplicant>>? Selector { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public JobApplicantSpecification(JobApplicantSpecParam param)
        {
            Criteria = x => (param.JobPostId == Guid.Empty || x.JobPostId == param.JobPostId) &&
                            (string.IsNullOrEmpty(param.Search) || x.UserApplied.Email.Contains(param.Search) || x.UserApplied.FullName.ToLower().Contains(param.Search.ToLower()) 
                            || EF.Functions.Like(EF.Functions.Unaccent(x.UserApplied.FullName).ToLower(), $"%{param.Search.ToLower()}%")) &&
                            (param.CategoryId == 0 || x.JobPost.CategoryId == param.CategoryId) &&
                            (param.IndustryId == 0 || x.JobPost.IndustryId == param.IndustryId) &&
                            (string.IsNullOrEmpty(param.CityId) || x.UserApplied.CityId == param.CityId) &&
                            (string.IsNullOrEmpty(param.WardId) || x.UserApplied.WardId == param.WardId);

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "appliedatasc":
                        OrderBy = q => q.OrderBy(x => x.AppliedAt);
                        break;
                    default:
                        OrderByDescending = q => q.OrderByDescending(x => x.AppliedAt);
                        break;
                }
            }

            Includes.Add(x => x.UserApplied);

            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;

        }
    }
}
