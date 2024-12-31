using MediatR;
using TimeSwap.Application.Location.Queries;
using TimeSwap.Application.Location.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Location.Handlers
{
    public class GetWardsByCityIdHandler : IRequestHandler<GetWardsByCityIdQuery, IEnumerable<WardResponse>>
    {
        private readonly IWardRepository _wardRepository;

        public GetWardsByCityIdHandler(IWardRepository wardRepository)
        {
            _wardRepository = wardRepository;
        }

        public async Task<IEnumerable<WardResponse>> Handle(GetWardsByCityIdQuery request, CancellationToken cancellationToken)
        {
            var wards = await _wardRepository.GetWardsByCityIdAsync(request.CityId);

            return AppMapper<CoreMappingProfile>.Mapper.Map<IEnumerable<WardResponse>>(wards);
        }
    }
}
