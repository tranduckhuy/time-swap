using MediatR;
using TimeSwap.Application.Exceptions.JobPost;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class GetJobPostByIdQueryHandler : IRequestHandler<GetJobPostByIdQuery, JobPostDetailResponse>
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IJobApplicantRepository _jobApplicantRepository;

        public GetJobPostByIdQueryHandler(IJobPostRepository jobPostRepository, IJobApplicantRepository jobApplicantRepository)
        {
            _jobPostRepository = jobPostRepository;
            _jobApplicantRepository = jobApplicantRepository;
        }

        public async Task<JobPostDetailResponse> Handle(GetJobPostByIdQuery request, CancellationToken cancellationToken)
        {
            var jobPost = await _jobPostRepository.GetJobPostByIdAsync(request.Id, cancellationToken)
                  ?? throw new JobPostNotFoundException();

            var totalApplicants = await _jobApplicantRepository.GetTotalApplicantsByJobPostIdAsync(request.Id, cancellationToken);
            var relatedJobPosts = await _jobPostRepository
                .GetRelatedJobPostsAsync(jobPost.Id, jobPost.Category.Id, jobPost.Industry.Id, AppConstant.RELATED_JOB_POSTS, cancellationToken);
            var isCurrentUserApplied = await _jobApplicantRepository.JobApplicantExistsAsync(jobPost.Id, request.UserId, cancellationToken);

            var jobPostResponse = AppMapper<CoreMappingProfile>.Mapper.Map<JobPostDetailResponse>(jobPost);

            jobPostResponse.TotalApplicants = totalApplicants;
            jobPostResponse.RelatedJobPosts = AppMapper<CoreMappingProfile>.Mapper.Map<IEnumerable<JobPostResponse>>(relatedJobPosts);
            jobPostResponse.IsCurrentUserApplied = isCurrentUserApplied;

            return jobPostResponse;
        }
    }
}
