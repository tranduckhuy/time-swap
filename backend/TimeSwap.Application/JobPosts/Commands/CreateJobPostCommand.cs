using MediatR;
using TimeSwap.Application.JobPosts.Responses;

namespace TimeSwap.Application.JobPosts.Commands
{
    public class CreateJobPostCommand : IRequest<JobPostResponse>
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Responsibilities { get; set; } = string.Empty;

        public decimal Fee { get; set; }
        
        public DateTime? StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int CategoryId { get; set; }

        public int IndustryId { get; set; }

        public string? WardId { get; set; }

        public string? CityId { get; set; }
    }
}
