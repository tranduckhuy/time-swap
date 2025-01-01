using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

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

        [HttpGet("{jobPostId}")]
        public async Task<IActionResult> GetJobPostById(Guid jobPostId)
        {
            var query = new GetJobPostByIdQuery(jobPostId);
            return await ExecuteAsync<GetJobPostByIdQuery, JobPostResponse>(query);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobPost([FromBody] CreateJobPostCommand command)
        {
            return await ExecuteAsync<CreateJobPostCommand, JobPostResponse>(command);
        }
    }
}
