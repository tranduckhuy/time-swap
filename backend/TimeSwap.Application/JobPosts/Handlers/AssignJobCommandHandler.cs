using MediatR;
using TimeSwap.Application.Exceptions.JobPost;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class AssignJobCommandHandler : IRequestHandler<AssignJobCommand, Unit>
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IJobApplicantRepository _jobApplicantRepository;

        public AssignJobCommandHandler(IJobPostRepository jobPostRepository, IJobApplicantRepository jobApplicantRepository)
        {
            _jobPostRepository = jobPostRepository;
            _jobApplicantRepository = jobApplicantRepository;
        }

        public async Task<Unit> Handle(AssignJobCommand request, CancellationToken cancellationToken)
        {
            var jobPost = await _jobPostRepository.GetJobPostByIdAsync(request.JobPostId) ?? throw new JobPostNotFoundException();

            if (jobPost.UserId != request.OwnerId)
            {
                throw new OwnerJobPostMismatchException();
            }

            if (jobPost.UserId == request.UserAppliedId)
            {
                throw new AssignJobToOwnerException();
            }

            if (jobPost.AssignedTo != null)
            {
                throw new JobPostAlreadyAssignedException();
            }

            if (!await _jobApplicantRepository.IsUserAppliedToJobPostAsync(request.JobPostId, request.UserAppliedId))
            {
                throw new UserNotAppliedToJobPostException();
            }

            jobPost.AssignedTo = request.UserAppliedId;
            jobPost.IsActive = false;

            await _jobPostRepository.UpdateJobPostAsync(jobPost);

            return Unit.Value;
        }
    }
}
