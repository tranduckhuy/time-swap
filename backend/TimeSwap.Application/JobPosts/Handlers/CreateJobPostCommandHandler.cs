using MediatR;
using Microsoft.Extensions.Logging;
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

        public CreateJobPostCommandHandler(
            IJobPostRepository jobPostRepository, 
            JobPostValidatorService jobPostValidatorService)
        {
            _jobPostRepository = jobPostRepository;
            _jobPostValidatorService = jobPostValidatorService;
        }

        public async Task<JobPostResponse> Handle(CreateJobPostCommand request, CancellationToken cancellationToken)
        {
            await _jobPostValidatorService.ValidateAsync(request);

            var jobPost = AppMapper<CoreMappingProfile>.Mapper.Map<JobPost>(request);

            await _jobPostRepository.CreateJobPostAsync(jobPost);

            var jobPostResponse = AppMapper<CoreMappingProfile>.Mapper.Map<JobPostResponse>(jobPost);

            return jobPostResponse;
        }
    }
}
