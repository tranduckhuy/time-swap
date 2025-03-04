using MediatR;
using TimeSwap.Application.JobPosts.Responses;

namespace TimeSwap.Application.JobPosts.Queries
{
    public record GetJobPostByIdQuery(Guid Id, Guid UserId) : IRequest<JobPostDetailResponse>;
}
