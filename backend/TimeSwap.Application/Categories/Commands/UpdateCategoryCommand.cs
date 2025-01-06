using MediatR;

namespace TimeSwap.Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int IndustryId { get; set; }
    }
}
