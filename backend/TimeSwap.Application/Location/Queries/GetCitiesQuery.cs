using MediatR;
using TimeSwap.Application.Location.Responses;

namespace TimeSwap.Application.Location.Queries
{
    public record GetCitiesQuery : IRequest<IEnumerable<CityResponse>> { }
}
