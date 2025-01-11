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
        private readonly IUserRepository _userRepository;

        public UpdateJobPostCommandHandler(
            IJobPostRepository jobPostRepository,
            JobPostValidatorService jobPostValidatorService,
            IUserRepository userRepository)
        {
            _jobPostRepository = jobPostRepository;
            _jobPostValidatorService = jobPostValidatorService;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateJobPostCommand request, CancellationToken cancellationToken)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(request.Id) ?? throw new JobPostNotFoundException();
            
            var feeDifference = await _jobPostValidatorService.ValidateUpdateJobPostAsync(jobPost, request);

            // If the fee is increased, deduct 10% of the difference as a service fee
            if (feeDifference > 0)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);

                user!.Balance -= feeDifference * (0.1m);
                await _userRepository.UpdateAsync(user);
            }

            AppMapper<CoreMappingProfile>.Mapper.Map(request, jobPost);

            await _jobPostRepository.UpdateAsync(jobPost);

            return Unit.Value;
        }
    }
}
