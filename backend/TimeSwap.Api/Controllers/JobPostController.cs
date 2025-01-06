using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TimeSwap.Api.Mapping;
using TimeSwap.Api.Models;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [Route("api/jobposts")]
    public class JobPostController : BaseController<JobPostController>
    {
        public JobPostController(
            IMediator mediator,
            ILogger<JobPostController> logger
        ) : base(mediator, logger) { }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<Pagination<JobPostResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetJobPosts([FromQuery] JobPostSpecParam jobPostSpecParam)
        {
            var query = new GetJobPostsQuery(jobPostSpecParam);
            return await ExecuteAsync<GetJobPostsQuery, Pagination<JobPostResponse>>(query);
        }

        [HttpGet("{jobPostId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<JobPostResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetJobPostById(Guid jobPostId)
        {
            var query = new GetJobPostByIdQuery(jobPostId);
            return await ExecuteAsync<GetJobPostByIdQuery, JobPostDetailResponse>(query);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<JobPostResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateJobPost([FromBody] CreateJobPostRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (request == null || string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields or the user id is not found in the claims"]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<CreateJobPostCommand>(request);
            command.UserId = Guid.Parse(userId);

            return await ExecuteAsync<CreateJobPostCommand, JobPostResponse>(command);
        }

        [HttpPut("{jobPostId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateJobPost(Guid jobPostId, [FromBody] UpdateJobPostRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (request == null || jobPostId != request.Id || string.IsNullOrEmpty(userId))
            {
                var errorMessage = string.IsNullOrEmpty(userId)
                    ? "The user id is not found in the claims"
                    : $"[The request body does not contain required fields] or " +
                    $"[The job post id in the request body ({request?.Id}) does not match the job post id in the route ({jobPostId})]";

                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = [errorMessage]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<UpdateJobPostCommand>(request);
            command.UserId = Guid.Parse(userId!);

            return await ExecuteAsync<UpdateJobPostCommand, Unit>(command);
        }

        [HttpPost("{jobPostId:guid}/apply")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Unit>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssignUserToJobPost(AssignJobPostRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (request == null || userId == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields or the user id is not found in the claims"]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<AssignJobCommand>(request);
            command.OwnerId = Guid.Parse(userId);

            return await ExecuteAsync<AssignJobCommand, Unit>(command);
        }

        [HttpGet("user/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetJobPostsByUserId(Guid userId, bool isOwner)
        {
            var query = new GetJobPostsByUserIdQuery(userId, isOwner);
            return await ExecuteAsync<GetJobPostsByUserIdQuery, IEnumerable<JobPostResponse>>(query);
        }
    }
}
