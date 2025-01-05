using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimeSwap.Application.Location.Queries;
using TimeSwap.Application.Location.Responses;
using TimeSwap.Shared;

namespace TimeSwap.Api.Controllers
{
    [Route("api/location")]
    public class LocationController : BaseController<LocationController>
    {
        public LocationController(
            IMediator mediator,
            ILogger<LocationController> logger
        ) : base(mediator, logger) { }

        [HttpGet("cities")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CityResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCities()
        {
            return await ExecuteAsync<GetCitiesQuery, IEnumerable<CityResponse>>(new GetCitiesQuery());
        }

        [HttpGet("cities/{cityId}/wards")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WardResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWardsByCityId(string cityId)
        {
            return await ExecuteAsync<GetWardsByCityIdQuery, IEnumerable<WardResponse>>(new GetWardsByCityIdQuery(cityId));
        }
    }
}
