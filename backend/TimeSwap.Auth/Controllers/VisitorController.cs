using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimeSwap.Application.Visistor;
using TimeSwap.Domain.Entities;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Controllers
{
    [ApiController]
    [Route("api/visitors")]
    public class VisitorController : ControllerBase
    {
        private readonly IVisitorService _visitorService;

        public VisitorController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MonthlyVisit>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVisitsInRange([FromQuery] int startYear, [FromQuery] int startMonth,
                                                          [FromQuery] int endYear, [FromQuery] int endMonth)
        {
            var visits = await _visitorService.GetVisitsInRange(startYear, startMonth, endYear, endMonth);


            return Ok(new ApiResponse<IEnumerable<MonthlyVisit>>
            {
                StatusCode = (int)Shared.Constants.StatusCode.RequestProcessedSuccessfully,
                Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.RequestProcessedSuccessfully),
                Data = visits
            });
        }
    }

}
