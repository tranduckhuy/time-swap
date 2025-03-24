using MediatR;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Validators;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class CreateJobPostCommandHandler : IRequestHandler<CreateJobPostCommand, JobPostResponse>
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly JobPostValidatorService _jobPostValidatorService;
        private readonly IUserRepository _userRepository;
        private readonly UserProfileValidatorService _userProfileValidatorService;

        public CreateJobPostCommandHandler(
            IJobPostRepository jobPostRepository,
            JobPostValidatorService jobPostValidatorService,
            IUserRepository userRepository,
            UserProfileValidatorService userProfileValidatorService)
        {
            _jobPostRepository = jobPostRepository;
            _jobPostValidatorService = jobPostValidatorService;
            _userRepository = userRepository;
            _userProfileValidatorService = userProfileValidatorService;
        }

        public async Task<JobPostResponse> Handle(CreateJobPostCommand request, CancellationToken cancellationToken)
        {
            await _jobPostValidatorService.ValidateCreateJobPostAsync(request);

            await _userProfileValidatorService.ValidateUserProfileAsync(request.UserId);

            var jobPost = AppMapper<CoreMappingProfile>.Mapper.Map<JobPost>(request);

            await _jobPostRepository.CreateJobPostAsync(jobPost);

            // Deduct 10% of the fee as a service fee
            var user = await _userRepository.GetByIdAsync(request.UserId);
            user!.Balance -= jobPost.Fee * (0.1m);
            await _userRepository.UpdateAsync(user);

            return AppMapper<CoreMappingProfile>.Mapper.Map<JobPostResponse>(jobPost);
        }
    }
}
