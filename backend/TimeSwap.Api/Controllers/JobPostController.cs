using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;
using TimeSwap.Shared.Constants;
using TimeSwap.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TimeSwap.Api.Models;
using TimeSwap.Application.Mappings;
using TimeSwap.Api.Mapping;

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
        public async Task<IActionResult> GetJobPosts([FromQuery] JobPostSpecParam jobPostSpecParam)
        {
            var query = new GetJobPostsQuery(jobPostSpecParam);
            return await ExecuteAsync<GetJobPostsQuery, Pagination<JobPostResponse>>(query);
        }

        [HttpGet("{jobPostId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetJobPostById(Guid jobPostId)
        {
            var query = new GetJobPostByIdQuery(jobPostId);
            return await ExecuteAsync<GetJobPostByIdQuery, JobPostResponse>(query);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateJobPost([FromBody] CreateJobPostRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (request == null || string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["Request body cannot be null or userId is not found in the claims"]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<CreateJobPostCommand>(request);
            command.UserId = Guid.Parse(userId);

            return await ExecuteAsync<CreateJobPostCommand, JobPostResponse>(command);
        }

        [HttpPut("{jobPostId:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateJobPost(Guid jobPostId, [FromBody] UpdateJobPostRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (request == null || jobPostId != request.Id || string.IsNullOrEmpty(userId))
            {
                var errorMessage = string.IsNullOrEmpty(userId)
                    ? "The user id is not found in the claims"
                    : $"Request body cannot be null or JobPostId: `{jobPostId}` does not match";

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
        public async Task<IActionResult> AssignUserToJobPost(AssignJobPostRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (request == null || userId == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = [$"Request body cannot be null or userId not exist"]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<AssignJobCommand>(request);
            command.OwnerId = Guid.Parse(userId);

            return await ExecuteAsync<AssignJobCommand, Unit>(command);
        }
    }
}
