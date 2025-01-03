using MediatR;
using TimeSwap.Application.JobApplicants.Responses;

namespace TimeSwap.Application.JobApplicants.Commands
{
    public class CreateJobApplicantCommand : IRequest<JobApplicantResponse>
    {
        public Guid UserId { get; set; }
        public Guid JobPostId { get; set; }
        public Guid UserAppliedId { get; set; }
    }
}
