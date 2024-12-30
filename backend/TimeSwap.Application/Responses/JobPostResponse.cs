using TimeSwap.Domain.Entities;

namespace TimeSwap.Application.Responses
{
    public class JobPostResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? AssignedTo { get; set; }
        public bool IsOwnerCompleted { get; set; }
        public bool IsAssigneeCompleted { get; set; }
        public Category Category {  get; set; } = null!;
        public Industry Industry { get; set; } = null!;
        public List<string> LocationIds { get; set; } = [];
    }
}
