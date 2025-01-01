using MediatR;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

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
                throw new AppException(StatusCode.IndustryNotFound);
            }

            return new IndustryResponse
            {
                Id = industry.Id,
                IndustryName = industry.IndustryName
            };
        }
    }
}
