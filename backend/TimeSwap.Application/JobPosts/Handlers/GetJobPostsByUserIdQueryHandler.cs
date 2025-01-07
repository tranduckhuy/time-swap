using MediatR;
using System.Linq.Expressions;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobPosts.Handlers
{
    public class GetJobPostsByUserIdQueryHandler : IRequestHandler<GetJobPostsByUserIdQuery, IEnumerable<JobPostResponse>>
    {
        private readonly IJobPostRepository _jobPostRepository;

        public GetJobPostsByUserIdQueryHandler(IJobPostRepository jobPostRepository)
        {
            _jobPostRepository = jobPostRepository;
        }

        public async Task<IEnumerable<JobPostResponse>> Handle(GetJobPostsByUserIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<JobPost, bool>> expression = request.IsOwner
                ? x => x.UserId == request.UserId && (x.IsOwnerCompleted || x.IsAssigneeCompleted)
                : x => x.AssignedTo == request.UserId && (x.IsOwnerCompleted || x.IsAssigneeCompleted);


            var jobPosts = await _jobPostRepository.GetJobPostsByUserIdAsync(expression);

            return AppMapper<CoreMappingProfile>.Mapper.Map<IEnumerable<JobPostResponse>>(jobPosts);
        }
    }
}
