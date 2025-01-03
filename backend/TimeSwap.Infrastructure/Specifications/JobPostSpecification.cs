﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Infrastructure.Specifications
{
    public class JobPostSpecification : ISpecification<JobPost>
    {
        public Expression<Func<JobPost, bool>> Criteria { get; private set; }
        public Func<IQueryable<JobPost>, IOrderedQueryable<JobPost>>? OrderBy { get; private set; }
        public Func<IQueryable<JobPost>, IOrderedQueryable<JobPost>>? OrderByDescending { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public List<Expression<Func<JobPost, object>>> Includes { get; private set; } = [];

        public JobPostSpecification(JobPostSpecParam param)
        {
            // Build Criteria (e.g., Search, Filters)
            Criteria = x =>
                (string.IsNullOrEmpty(param.Search) || EF.Functions.Like(EF.Functions.Unaccent(x.Title).ToLower(), $"%{param.Search.ToLower()}%")) &&
                (param.IndustryId == 0 || x.IndustryId == param.IndustryId) &&
                (param.CategoryId == 0 || x.CategoryId == param.CategoryId) &&
                (param.MinFee == null || x.Fee >= param.MinFee) &&
                (param.MaxFee == null || x.Fee <= param.MaxFee) &&
                (param.PostedDate == null || x.CreatedAt >= param.PostedDate) &&
                (string.IsNullOrEmpty(param.CityId) || x.CityId == param.CityId) &&
                (string.IsNullOrEmpty(param.WardId) || x.WardId == param.WardId) &&
                (x.IsActive == param.IsActive);

            // Sorting logic
            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort.ToLower())
                {
                    case "feeasc":
                        OrderBy = q => q.OrderBy(x => x.Fee);
                        break;
                    case "feedesc":
                        OrderByDescending = q => q.OrderByDescending(x => x.Fee);
                        break;
                    default:
                        OrderBy = q => q.OrderBy(x => x.Title);
                        break;
                }
            }

            // Include navigation properties
            Includes.Add(x => x.Category);
            Includes.Add(x => x.Industry);
            Includes.Add(x => x.Ward);

            // Pagination
            Skip = (param.PageIndex - 1) * param.PageSize;
            Take = param.PageSize;
        }
    }
}
