using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Application.Industries.Commands
{
    public class UpdateIndustryCommand : IRequest<Unit>
    {
        [Required(ErrorMessage = "Industry Id is required.")]
        public int IndustryId { get; set; }

        [Required(ErrorMessage = "Industry name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Industry name must be between 1 and 255 characters.")]
        public string IndustryName { get; set; } = string.Empty;
    }
}
