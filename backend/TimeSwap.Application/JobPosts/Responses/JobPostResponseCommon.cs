using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.Location.Responses;

namespace TimeSwap.Application.JobPosts.Responses
{
    public abstract class JobPostResponseCommon
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? OwnerAvatarUrl { get; set; } = string.Empty;
        public string? OwnerName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? AssignedTo { get; set; }
        public bool IsOwnerCompleted { get; set; }
        public bool IsAssigneeCompleted { get; set; }
        public CategoryResponse Category { get; set; } = null!;
        public IndustryResponse Industry { get; set; } = null!;
        public WardResponse Ward { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
