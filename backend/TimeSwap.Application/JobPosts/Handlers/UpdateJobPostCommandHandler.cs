using MediatR;
using TimeSwap.Application.Exceptions.JobPost;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Validators;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class UpdateJobPostCommandHandler : IRequestHandler<UpdateJobPostCommand, Unit>
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly JobPostValidatorService _jobPostValidatorService;

        public UpdateJobPostCommandHandler(IJobPostRepository jobPostRepository, JobPostValidatorService jobPostValidatorService)
        {
            _jobPostRepository = jobPostRepository;
            _jobPostValidatorService = jobPostValidatorService;
        }

        public async Task<Unit> Handle(UpdateJobPostCommand request, CancellationToken cancellationToken)
        {
            await _jobPostValidatorService.ValidateUpdateJobPostAsync(request);

            var jobPost = await _jobPostRepository.GetByIdAsync(request.Id) ?? throw new JobPostNotFoundException();

            AppMapper<CoreMappingProfile>.Mapper.Map(request, jobPost);

            await _jobPostRepository.UpdateAsync(jobPost);

            return Unit.Value;
        }
    }
}
