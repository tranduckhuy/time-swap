using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Api.Mapping;
using TimeSwap.Api.Models;
using TimeSwap.Application.Categories.Responses;
using TimeSwap.Application.Industries.Commands;
using TimeSwap.Application.Industries.Queries;
using TimeSwap.Application.Industries.Responses;
using TimeSwap.Application.Mappings;
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
            ILogger<IndustryController> logger
        ) : base(mediator, logger) { }

        [HttpGet]
        public async Task<IActionResult> GetIndustries()
        {
            var query = new GetIndustriesQuery();
            return await ExecuteAsync<GetIndustriesQuery, List<IndustryResponse>>(query);
        }

        [HttpGet("{industryId}")]
        public async Task<IActionResult> GetIndustryById(int industryId)
        {
            var query = new GetIndustryByIdQuery(industryId);
            return await ExecuteAsync<GetIndustryByIdQuery, IndustryResponse>(query);
        }

        [HttpGet("{industryId}/categories")]
        public async Task<IActionResult> GetCategoriesByIndustry(int industryId)
        {
            var query = new GetCategoriesByIndustryQuery
            {
                IndustryId = industryId,
            };

            return await ExecuteAsync<GetCategoriesByIndustryQuery, List<CategoryResponse>>(query);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> CreateIndustry([FromBody] CreateIndustryRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields"]
                });
            }
            var command = AppMapper<ModelMapping>.Mapper.Map<CreateIndustryCommand>(request);
            return await ExecuteAsync<CreateIndustryCommand, int>(command);
        }

        [HttpPut("{industryId}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> UpdateIndustry([FromBody] UpdateIndustryRequest request, int industryId)
        {
            if (request == null || request.IndustryId != industryId)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body is invalid or does not match the industry ID in the route."]
                });
            }
            var command = AppMapper<ModelMapping>.Mapper.Map<UpdateIndustryCommand>(request);
            return await ExecuteAsync<UpdateIndustryCommand, Unit>(command);
        }
    }
}
