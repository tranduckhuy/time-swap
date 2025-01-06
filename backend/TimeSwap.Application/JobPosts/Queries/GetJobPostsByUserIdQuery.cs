using MediatR;
using TimeSwap.Application.JobPosts.Responses;

namespace TimeSwap.Application.JobPosts.Queries
{
    public record GetJobPostsByUserIdQuery(Guid UserId, bool IsOwner) 
        : IRequest<IEnumerable<JobPostResponse>>
    {
    }
}
