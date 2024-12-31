using MediatR;
using TimeSwap.Application.Location.Queries;
using TimeSwap.Application.Location.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Location.Handlers
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesQuery, IEnumerable<CityResponse>>
    {
        private readonly ICityRepository _cityRepository;

        public GetCitiesHandler(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<CityResponse>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _cityRepository.GetAllAsync();

            return AppMapper<CoreMappingProfile>.Mapper.Map<IEnumerable<CityResponse>>(cities);
        }
    }
}
