using MediatR;

namespace TimeSwap.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public int IndustryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
