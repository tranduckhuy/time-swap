using System.Linq.Expressions;
using TimeSwap.Domain.Entities;

namespace TimeSwap.Infrastructure.Projections
{
    public static class JobPostProjections
    {
        public static Expression<Func<JobPost, JobPost>> SelectJobPostProjection()
        {
            return x => new JobPost
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Description = x.Description,
                Responsibilities = x.Responsibilities,
                Fee = x.Fee,
                StartDate = x.StartDate,
                DueDate = x.DueDate,
                AssignedTo = x.AssignedTo,
                IsOwnerCompleted = x.IsOwnerCompleted,
                IsAssigneeCompleted = x.IsAssigneeCompleted,
                Category = x.Category,
                Industry = x.Industry,
                Ward = x.Ward,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt,
                User = new UserProfile
                {
                    AvatarUrl = x.User.AvatarUrl,
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    Ward = x.User.Ward,
                }
            };
        }
    }
}
