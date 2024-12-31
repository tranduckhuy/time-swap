using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Application.Location.Queries;
using TimeSwap.Application.Location.Responses;

namespace TimeSwap.Api.Controllers
{
    public class LocationController : BaseController
    {
        public LocationController(IMediator mediator) : base(mediator) { }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            return await ExecuteAsync<GetCitiesQuery, IEnumerable<CityResponse>>(new GetCitiesQuery());
        }

        [HttpGet("cities/{cityId}/wards")]
        public async Task<IActionResult> GetWardsByCityId(string cityId)
        {
            return await ExecuteAsync<GetWardsByCityIdQuery, IEnumerable<WardResponse>>(new GetWardsByCityIdQuery(cityId));
        }
    }
}
