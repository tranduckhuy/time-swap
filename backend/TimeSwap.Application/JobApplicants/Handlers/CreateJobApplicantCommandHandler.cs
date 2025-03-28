﻿using MediatR;
using TimeSwap.Application.Exceptions.JobPost;
using TimeSwap.Application.JobApplicants.Commands;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Validators;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobApplicants.Handlers
{
    public class CreateJobApplicantCommandHandler : IRequestHandler<CreateJobApplicantCommand, JobApplicantResponse>
    {
        private readonly IJobApplicantRepository _jobApplicantRepository;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly UserProfileValidatorService _userProfileValidatorService;

        public CreateJobApplicantCommandHandler(IJobApplicantRepository jobApplicantRepository,
            IJobPostRepository jobPostRepository,
            UserProfileValidatorService userProfileValidatorService)
        {
            _jobApplicantRepository = jobApplicantRepository;
            _jobPostRepository = jobPostRepository;
            _userProfileValidatorService = userProfileValidatorService;
        }

        public async Task<JobApplicantResponse> Handle(CreateJobApplicantCommand request, CancellationToken cancellationToken)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(request.JobPostId) ?? throw new JobPostNotFoundException();

            // check if all information of userprofile is completed
            await _userProfileValidatorService.ValidateUserProfileAsync(request.UserId);

            if (jobPost.UserId == request.UserId)
            {
                throw new UserAppliedToOwnJobPostException();
            }

            // check if user already applied to this job post
            var jobApplicantExists = await _jobApplicantRepository.JobApplicantExistsAsync(request.JobPostId, request.UserId, cancellationToken);
            if (jobApplicantExists)
            {
                throw new JobApplicantAlreadyExistsException();
            }

            var jobApplicant = new JobApplicant
            {
                JobPostId = request.JobPostId,
                UserAppliedId = request.UserId
            };

            var jobApplicantCreated = await _jobApplicantRepository.AddAsync(jobApplicant);

            return AppMapper<CoreMappingProfile>.Mapper.Map<JobApplicantResponse>(jobApplicantCreated);
        }
    }
}
