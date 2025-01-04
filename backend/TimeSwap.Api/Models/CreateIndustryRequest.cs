using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Api.Models
{
    public class CreateIndustryRequest
    {
        [Required(ErrorMessage = "Industry Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Industry Name must be between 1 and 255 characters.")]
        public string IndustryName { get; set; } = string.Empty;
    }
}
