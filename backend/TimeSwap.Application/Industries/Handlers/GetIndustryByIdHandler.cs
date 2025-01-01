using MediatR;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
{
    public class GetIndustryByIdHandler : IRequestHandler<GetIndustryByIdQuery, IndustryResponse>
    {
        private readonly IIndustryRepository _industryRepository;

        public GetIndustryByIdHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        public async Task<IndustryResponse> Handle(GetIndustryByIdQuery request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.GetByIdAsync(request.Id);
            if (industry == null)
            {
                throw new IndustryNotFoundException();
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<IndustryResponse>(industry);
        }
    }
}
