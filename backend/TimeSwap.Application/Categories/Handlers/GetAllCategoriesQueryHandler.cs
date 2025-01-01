using MediatR;
using TimeSwap.Application.Categories.Queries;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Categories.Handlers
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsyncIndustry();

            return categories.Select(c => new CategoryResponse
            {
                CategoryId = c.Id,
                CategoryName = c.CategoryName,
                IndustryId = c.IndustryId,
                IndustryName = c.Industry.IndustryName
            }).ToList();
        }
    }
}
