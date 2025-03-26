using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Net;
using System.Security.Claims;
using TimeSwap.Api.Mapping;
using TimeSwap.Api.Models;
using TimeSwap.Application.JobApplicants.Commands;
using TimeSwap.Application.JobApplicants.Queries;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<JobApplicantResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetApplicantsByJobPostId(Guid jobPostId, [FromQuery] JobApplicantSpecParam jobApplicantSpecParam)
        {
            var query = new GetJobApplicantsQuery(jobApplicantSpecParam);
            query.JobApplicantSpecParam.JobPostId = jobPostId;

            return await ExecuteAsync<GetJobApplicantsQuery, Pagination<JobApplicantResponse>>(query);
        }

        [HttpGet("odata")]
        [EnableQuery]
        public async Task<IActionResult> GetApplicants()
        {
            var jobApplicantSpecParam = new JobApplicantSpecParam();
            var query = new GetJobApplicantsQuery(jobApplicantSpecParam);
            var result = await _mediator.Send(query);
            return Ok(result.Data);
        }


        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<JobApplicantResponse>), (int)HttpStatusCode.OK)]
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
