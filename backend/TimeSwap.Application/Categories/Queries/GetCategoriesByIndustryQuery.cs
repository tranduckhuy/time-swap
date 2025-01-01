using MediatR;
using TimeSwap.Application.Categories.Responses;

namespace TimeSwap.Application.Categories.Queries
{
    public class GetCategoriesByIndustryQuery : IRequest<List<CategoryResponse>>
    {
        public int IndustryId { get; set; }
        public int PageIndex { get; set; } = 1; 
        public int PageSize { get; set; } = 10; 
    }
}
