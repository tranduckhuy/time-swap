using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 255 characters.")]
        public string CategoryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "IndustryId is required.")]
        public int IndustryId { get; set; }
    }
}
