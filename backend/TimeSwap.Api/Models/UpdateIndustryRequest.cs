using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimeSwap.Api.Models
{
    public class UpdateIndustryRequest
    {
        [JsonRequired]
        public int IndustryId { get; set; }

        [Required(ErrorMessage = "Industry name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Industry name must be between 1 and 255 characters.")]
        public string IndustryName { get; set; } = string.Empty;
    }
}
