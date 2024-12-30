using MediatR;
using TimeSwap.Application.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Application.Queries
{
    public class GetJobPostsQuery(JobPostSpecParam jobPostSpecParam) : IRequest<Pagination<JobPostResponse>>
    {
        public JobPostSpecParam JobPostSpecParam { get; set; } = jobPostSpecParam;
    }
}
