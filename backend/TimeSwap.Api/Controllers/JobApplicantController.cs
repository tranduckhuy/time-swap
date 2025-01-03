using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeSwap.Api.Mapping;
using TimeSwap.Api.Models;
using TimeSwap.Application.JobApplicants.Commands;
using TimeSwap.Application.JobApplicants.Queries;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [ApiController]
    [Route("api/applicants")]
    public class JobApplicantController : BaseController<JobApplicantController>
    {
        public JobApplicantController(
            IMediator mediator,
            ILogger<JobApplicantController> logger
        ) : base(mediator, logger)
        {
        }

        [HttpGet("{jobPostId}")]
        [Authorize]
        public async Task<IActionResult> GetApplicantsByJobPostId(Guid jobPostId)
        {
            var query = new GetApplicantsByJobPostIdQuery(jobPostId);
            return await ExecuteAsync<GetApplicantsByJobPostIdQuery, IEnumerable<JobApplicantResponse>>(query);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateJobApplicant([FromBody] CreateJobApplicantRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (request == null || string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["Request body cannot be null or user id is not found in the claims"]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<CreateJobApplicantCommand>(request);
            command.UserId = Guid.Parse(userId);

            return await ExecuteAsync<CreateJobApplicantCommand, JobApplicantResponse>(command);
        }
    }
}
