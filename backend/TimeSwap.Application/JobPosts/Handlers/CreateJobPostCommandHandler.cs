using MediatR;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.JobPosts.Validators;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class CreateJobPostCommandHandler : IRequestHandler<CreateJobPostCommand, JobPostResponse>
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly JobPostValidatorService _jobPostValidatorService;
        private readonly IUserRepository _userRepository;

        public CreateJobPostCommandHandler(
            IJobPostRepository jobPostRepository,
            JobPostValidatorService jobPostValidatorService,
            IUserRepository userRepository)
        {
            _jobPostRepository = jobPostRepository;
            _jobPostValidatorService = jobPostValidatorService;
            _userRepository = userRepository;
        }

        public async Task<JobPostResponse> Handle(CreateJobPostCommand request, CancellationToken cancellationToken)
        {
            await _jobPostValidatorService.ValidateCreateJobPostAsync(request);

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
