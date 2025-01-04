using MediatR;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Industries.Queries
{
    public class GetCategoriesByIndustryQuery : IRequest<Pagination<CategoryResponse>>
    {
        public int IndustryId { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
