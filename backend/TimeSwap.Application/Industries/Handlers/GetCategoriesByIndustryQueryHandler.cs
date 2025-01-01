using MediatR;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Exceptions.Categories;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
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
                throw new CategoryNotFoundByIndustryIdException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<CategoryResponse>>(paginationResult.Data);
        }
    }
}
