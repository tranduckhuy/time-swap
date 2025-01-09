using MediatR;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class GetJobPostsByUserIdWithPaginationQueryHandler 
        : IRequestHandler<GetJobPostsByUserIdWithPaginationQuery, Pagination<JobPostResponse>>
    {
        private readonly IJobPostRepository _jobPostRepository;

        public GetJobPostsByUserIdWithPaginationQueryHandler(IJobPostRepository jobPostRepository)
        {
            _jobPostRepository = jobPostRepository;
        }

        public async Task<Pagination<JobPostResponse>> Handle(GetJobPostsByUserIdWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var jobPosts = await _jobPostRepository.GetJobPostsByUserIdWithSpecAsync(request.JobPostByUserSpecParam);
            
            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<JobPostResponse>>(jobPosts);
        }
    }
}
