using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimeSwap.Api.Models
{
    public class CreateCategoryRequest
    {
        [JsonRequired]
        public int IndustryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 255 characters.")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
