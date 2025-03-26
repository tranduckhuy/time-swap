using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.User;

namespace TimeSwap.Infrastructure.Specifications.User
{
    public class UserSpecification : ISpecification<UserProfile>
    {
        public Expression<Func<UserProfile, bool>> Criteria { get; private set; }
        public Func<IQueryable<UserProfile>, IOrderedQueryable<UserProfile>>? OrderBy { get; private set; }
        public Func<IQueryable<UserProfile>, IOrderedQueryable<UserProfile>>? OrderByDescending { get; private set; }
        public List<Expression<Func<UserProfile, object>>> Includes { get; private set; } = [];
        public Func<IQueryable<UserProfile>, IQueryable<UserProfile>>? Selector { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }

        public UserSpecification(UserSpecParam param)
        {
            // Build Criteria (e.g., Search, Filters)
            Criteria = x =>
                (string.IsNullOrEmpty(param.Search) ||
                 EF.Functions.Like(EF.Functions.Unaccent(x.FullName).ToLower(), $"%{param.Search.ToLower()}%") ||
                 x.FullName.ToLower().Contains(param.Search.ToLower()) ||
                 x.Email.ToLower().Contains(param.Search.ToLower()));

            // Sorting logic
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "fullnameasc":
                        OrderBy = q => q.OrderBy(x => x.FullName);
                        break;
                    case "fullnamedesc":
                        OrderByDescending = q => q.OrderByDescending(x => x.FullName);
                        break;
                    default:
                        OrderBy = q => q.OrderBy(x => x.CreatedAt);
                        break;
                }
            }
            // Includes
            Includes.Add(x => x.MajorCategory);
            Includes.Add(x => x.MajorIndustry);
            Includes.Add(x => x.Ward);
            Includes.Add(x => x.City);

            // Pagination
            Skip = param.PageSize * (param.PageIndex - 1);
            Take = param.PageSize;
        }

    }
}
