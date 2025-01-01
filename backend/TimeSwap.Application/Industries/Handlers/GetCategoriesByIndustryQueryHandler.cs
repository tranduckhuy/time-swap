using MediatR;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.Industries.Handlers
{
    public class GetCategoriesByIndustryQueryHandler : IRequestHandler<GetCategoriesByIndustryQuery, Pagination<CategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIndustryRepository _industryRepository;

        public GetCategoriesByIndustryQueryHandler(
            ICategoryRepository categoryRepository,
            IIndustryRepository industryRepository)
        {
            _categoryRepository = categoryRepository;
            _industryRepository = industryRepository;
        }

        public async Task<Pagination<CategoryResponse>> Handle(GetCategoriesByIndustryQuery request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.GetByIdAsync(request.IndustryId) ?? throw new IndustryNotFoundException();

            var paginationResult = await _categoryRepository
                .GetCategoriesByIndustryAsync(request.IndustryId, request.PageIndex, request.PageSize);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<CategoryResponse>>(paginationResult);
        }
    }
}
