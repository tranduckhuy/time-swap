using MediatR;
using TimeSwap.Application.Exceptions.JobPost;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

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
            var jobPost = await _jobPostRepository.GetJobPostByIdAsync(request.Id) ?? throw new JobPostNotFoundException();

            var toalApplicants = await _jobApplicantRepository.GetTotalApplicantsByJobPostIdAsync(request.Id);

            var jobPostResponse = AppMapper<CoreMappingProfile>.Mapper.Map<JobPostDetailResponse>(jobPost);

            jobPostResponse.TotalApplicants = toalApplicants;

            var relatedJobPosts = await _jobPostRepository.GetRelatedJobPostsAsync(jobPost.Id, jobPost.Category.Id, jobPost.Industry.Id, 3);

            jobPostResponse.RelatedJobPosts = AppMapper<CoreMappingProfile>.Mapper.Map<IEnumerable<JobPostResponse>>(relatedJobPosts);

            return jobPostResponse;
        }
    }
}
