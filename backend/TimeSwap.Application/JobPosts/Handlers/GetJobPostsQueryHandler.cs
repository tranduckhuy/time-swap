﻿using MediatR;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class GetJobPostsQueryHandler : IRequestHandler<GetJobPostsQuery, Pagination<JobPostResponse>>
    {
        private readonly IJobPostRepository _jobPostRepository;

        public GetJobPostsQueryHandler(IJobPostRepository jobPostRepository)
        {
            _jobPostRepository = jobPostRepository;
        }

        public async Task<Pagination<JobPostResponse>> Handle(GetJobPostsQuery request, CancellationToken cancellationToken)
        {
            var jobPosts = await _jobPostRepository.GetJobPostsWithSpecAsync(request.JobPostSpecParam);

            var jobs = AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<JobPostResponse>>(jobPosts);
            return jobs;
        }
    }
}
