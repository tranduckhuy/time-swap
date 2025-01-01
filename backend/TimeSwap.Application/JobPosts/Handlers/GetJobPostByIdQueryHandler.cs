using MediatR;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class GetJobPostByIdQueryHandler : IRequestHandler<GetJobPostByIdQuery, JobPostResponse>
    {
        private readonly IJobPostRepository _jobPostRepository;

        public GetJobPostByIdQueryHandler(IJobPostRepository jobPostRepository)
        {
            _jobPostRepository = jobPostRepository;
        }

        public async Task<JobPostResponse> Handle(GetJobPostByIdQuery request, CancellationToken cancellationToken)
        {
            var jobPost = await _jobPostRepository.GetJobPostByIdAsync(request.Id);

            var jobPostResponse = AppMapper<CoreMappingProfile>.Mapper.Map<JobPostResponse>(jobPost);
            
            return jobPostResponse;
        }
    }
}
