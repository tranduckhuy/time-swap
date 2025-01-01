using MediatR;
using TimeSwap.Application.Categories.Responses;

namespace TimeSwap.Application.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryResponse>>
    {
    }
}
