﻿using System.Linq.Expressions;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Domain.Interfaces.Repositories
{
    public interface IJobPostRepository : IAsyncRepository<JobPost, Guid>
    {
        Task<Pagination<JobPost>?> GetJobPostsWithSpecAsync(JobPostSpecParam param);
        Task<JobPost?> GetJobPostByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<JobPost> CreateJobPostAsync(JobPost jobPost);
        Task UpdateJobPostAsync(JobPost jobPost);
        Task<int> GetUserJobPostCountOnCurrentDayAsync(Guid userId);
        Task<IEnumerable<JobPost>> GetRelatedJobPostsAsync(Guid jobPostId, int categoryId, int industryId, int limit, CancellationToken cancellationToken);
        Task<IEnumerable<JobPost>> GetJobPostsByUserIdAsync(Expression<Func<JobPost, bool>> expression);
        Task<Pagination<JobPost>?> GetJobPostsByUserIdWithSpecAsync(JobPostByUserSpecParam param);
    }
}
