using MediatR;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.Job;

namespace TimeSwap.Application.JobApplicants.Queries
{
    public record GetJobApplicantsQuery(JobApplicantSpecParam JobApplicantSpecParam) : IRequest<Pagination<JobApplicantResponse>>;
}
