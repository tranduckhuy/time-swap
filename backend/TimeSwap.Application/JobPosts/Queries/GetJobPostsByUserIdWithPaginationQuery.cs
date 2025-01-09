using MediatR;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Application.JobPosts.Queries
{
    public record GetJobPostsByUserIdWithPaginationQuery(JobPostByUserSpecParam JobPostByUserSpecParam) 
        : IRequest<Pagination<JobPostResponse>>;
}
