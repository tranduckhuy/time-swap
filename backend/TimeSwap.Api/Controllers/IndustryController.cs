using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Industries.Commands;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [Route("api/industries")]
    [ApiController]
    public class IndustryController : BaseController<IndustryController>
    {
        public IndustryController(
            IMediator mediator,
            ILogger<BaseController<IndustryController>> logger
        ) : base(mediator, logger) { }

        [HttpGet]
        public async Task<IActionResult> GetIndustries()
        {
            var query = new GetIndustriesQuery();
            return await ExecuteAsync<GetIndustriesQuery, List<IndustryResponse>>(query);
        }

        [HttpGet("{industryId}")]
        public async Task<IActionResult> GetIndustryById(int industryId = 1)
        {
            var query = new GetIndustryByIdQuery(industryId);
            return await ExecuteAsync<GetIndustryByIdQuery, IndustryResponse>(query);
        }

        [HttpGet("{industryId}/categories")]
        public async Task<IActionResult> GetCategoriesByIndustry(int industryId = 1, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetCategoriesByIndustryQuery
            {
                IndustryId = industryId,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return await ExecuteAsync<GetCategoriesByIndustryQuery, Pagination<CategoryResponse>>(query);
        }

        [HttpPut("{industryId}")]
        public async Task<IActionResult> UpdateIndustry([FromBody] UpdateIndustryCommand command, int industryId = 1)
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
            command.IndustryId = industryId;
            return await ExecuteAsync<UpdateIndustryCommand, Unit>(command);
        }

    }
}
