using MediatR;
using System.ComponentModel.DataAnnotations;
using TimeSwap.Application.JobPosts.Responses;

namespace TimeSwap.Application.JobPosts.Commands
{
    public class CreateJobPostCommand : IRequest<JobPostResponse>
    {
        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Range(50000, double.MaxValue, ErrorMessage = "Fee must be at least 50,000")]
        public decimal Fee { get; set; }
        
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "IndustryId is required")]
        public int IndustryId { get; set; }

        public string? WardId { get; set; }

        public string? CityId { get; set; }
    }
}
