using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Application.Industries.Commands;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [Route("api/industries")]
    [ApiController]
    public class IndustryController : BaseController
    {
        public IndustryController(IMediator mediator) : base(mediator) { }

        [HttpGet("all")]
        public async Task<IActionResult> GetIndustries()
        {
            var query = new GetIndustriesQuery();
            return await ExecuteAsync<GetIndustriesQuery, List<IndustryResponse>>(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIndustryById(int id = 1)
        {
            var query = new GetIndustryByIdQuery(id);
            return await ExecuteAsync<GetIndustryByIdQuery, IndustryResponse>(query);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIndustry([FromBody] UpdateIndustryCommand command, int id = 1)
        {
            if (command == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["Request body cannot be null"]
                });
            }
            command.IndustryId = id;
            return await ExecuteAsync<UpdateIndustryCommand, bool>(command);
        }

    }
}
