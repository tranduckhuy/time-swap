using MediatR;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
{
    public class GetIndustriesQueryHandler : IRequestHandler<GetIndustriesQuery, List<IndustryResponse>>
    {
        private readonly IIndustryRepository _industryRepository;


        public GetIndustriesQueryHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        public async Task<List<IndustryResponse>> Handle(GetIndustriesQuery request, CancellationToken cancellationToken)
        {
            var industries = await _industryRepository.GetAllAsync();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<IndustryResponse>>(industries);
        }
    }
}
