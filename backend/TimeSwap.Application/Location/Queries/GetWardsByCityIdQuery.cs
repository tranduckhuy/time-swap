using MediatR;
using TimeSwap.Application.Location.Responses;

namespace TimeSwap.Application.Location.Queries
{
    public record GetWardsByCityIdQuery(string CityId) : IRequest<IEnumerable<WardResponse>>;
}
