using MediatR;
using TimeSwap.Application.JobApplicants.Queries;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobApplicants.Handlers
{
    public class GetApplicantsByJobPostIdQueryHandler : IRequestHandler<GetApplicantsByJobPostIdQuery, IEnumerable<JobApplicantResponse>>
    {
        private readonly IJobApplicantRepository _jobApplicantRepository;

        public GetApplicantsByJobPostIdQueryHandler(IJobApplicantRepository jobApplicantRepository)
        {
            _jobApplicantRepository = jobApplicantRepository;
        }

        public async Task<IEnumerable<JobApplicantResponse>> Handle(GetApplicantsByJobPostIdQuery request, CancellationToken cancellationToken)
        {
            var jobApplicants = await _jobApplicantRepository.GetApplicantsByJobPostIdAsync(request.JobPostId);

            return AppMapper<CoreMappingProfile>.Mapper.Map<IEnumerable<JobApplicantResponse>>(jobApplicants);
        }
    }
}
