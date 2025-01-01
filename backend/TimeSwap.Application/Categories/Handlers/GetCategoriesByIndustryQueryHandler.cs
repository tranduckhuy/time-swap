using MediatR;
using TimeSwap.Application.Categories.Queries;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Categories.Handlers
{
    public class GetCategoriesByIndustryQueryHandler : IRequestHandler<GetCategoriesByIndustryQuery, List<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesByIndustryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponse>> Handle(GetCategoriesByIndustryQuery request, CancellationToken cancellationToken)
        {
            var paginationResult = await _categoryRepository.GetCategoriesByIndustryAsync(request.IndustryId, request.PageIndex, request.PageSize);

            if (paginationResult.Data == null || !paginationResult.Data.Any())
            {
                throw new AppException(StatusCode.CategoryNotFoundByIndustryId);
            }

            return paginationResult.Data.Select(c => new CategoryResponse
            {
                CategoryId = c.Id,
                CategoryName = c.CategoryName,
                IndustryId = c.IndustryId,
                IndustryName = c.Industry.IndustryName
            }).ToList();
        }
    }
}
