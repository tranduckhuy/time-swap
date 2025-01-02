using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Api.Models
{
    public abstract class JobPostCommand
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        public decimal Fee { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "IndustryId is required")]
        public int IndustryId { get; set; }

        public string? WardId { get; set; }

        public string? CityId { get; set; }
    }
}
