using MediatR;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
{
    public class GetCategoriesByIndustryQueryHandler : IRequestHandler<GetCategoriesByIndustryQuery, List<CategoryResponse>>
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

        public async Task<List<CategoryResponse>> Handle(GetCategoriesByIndustryQuery request, CancellationToken cancellationToken)
        {
            _ = await _industryRepository.GetByIdAsync(request.IndustryId) ?? throw new IndustryNotFoundException();

            var category = await _categoryRepository
                .GetCategoriesByIndustryAsync(request.IndustryId);

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<CategoryResponse>>(category);
        }
    }
}
