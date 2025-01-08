using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimeSwap.Api.Models
{
    public abstract class JobPostCommand
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Responsibilities is required")]
        public string Responsibilities { get; set; } = string.Empty;

        [JsonRequired]
        public decimal Fee { get; set; }

        public DateTime? StartDate { get; set; }

        [JsonRequired]
        public DateTime DueDate { get; set; }

        [JsonRequired]
        public int CategoryId { get; set; }

        [JsonRequired]
        public int IndustryId { get; set; }

        public string? WardId { get; set; }

        public string? CityId { get; set; }
    }
}
