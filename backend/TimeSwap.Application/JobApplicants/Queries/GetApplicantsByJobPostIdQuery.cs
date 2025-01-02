using MediatR;
using TimeSwap.Application.JobApplicants.Responses;

namespace TimeSwap.Application.JobApplicants.Queries
{
    public record GetApplicantsByJobPostIdQuery(Guid JobPostId) : IRequest<IEnumerable<JobApplicantResponse>>;
}
