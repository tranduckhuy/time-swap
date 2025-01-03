using MediatR;
using TimeSwap.Application.JobPosts.Responses;

namespace TimeSwap.Application.JobPosts.Queries
{
    public record GetJobPostByIdQuery(Guid Id) : IRequest<JobPostResponse>;
}
