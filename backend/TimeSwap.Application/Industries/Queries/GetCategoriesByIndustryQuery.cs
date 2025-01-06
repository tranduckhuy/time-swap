using MediatR;
using TimeSwap.Application.Categories.Responses;

namespace TimeSwap.Application.Industries.Queries
{
    public class GetCategoriesByIndustryQuery : IRequest<List<CategoryResponse>>
    {
        public int IndustryId { get; set; }
    }
}
