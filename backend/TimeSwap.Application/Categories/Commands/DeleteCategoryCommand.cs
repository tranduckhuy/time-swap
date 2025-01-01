using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Application.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        [Required]
        public int CategoryId { get; set; }
    }
}
