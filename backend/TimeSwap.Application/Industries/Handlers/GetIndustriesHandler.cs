using MediatR;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
{
    public class GetIndustriesHandler : IRequestHandler<GetIndustriesQuery, List<IndustryResponse>>
    {
        private readonly IIndustryRepository _industryRepository;

        public GetIndustriesHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        public async Task<List<IndustryResponse>> Handle(GetIndustriesQuery request, CancellationToken cancellationToken)
        {
            var industries = await _industryRepository.GetAllAsync();
            return industries.Select(i => new IndustryResponse
            {
                Id = i.Id,
                IndustryName = i.IndustryName
            }).ToList();
        }
    }
}
