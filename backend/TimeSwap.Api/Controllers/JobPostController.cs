using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Api.Controllers
{
    [Route("api/jobposts")]
    public class JobPostController : BaseController
    {
        public JobPostController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetJobPosts([FromQuery] JobPostSpecParam jobPostSpecParam)
        {
            var query = new GetJobPostsQuery(jobPostSpecParam);
            return await ExecuteAsync<GetJobPostsQuery, Pagination<JobPostResponse>>(query);
        }
    }
}
