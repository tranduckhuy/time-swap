using MediatR;
using TimeSwap.Application.JobApplicants.Queries;
using TimeSwap.Application.JobApplicants.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;

namespace TimeSwap.Application.JobApplicants.Handlers
{
    public class GetJobApplicantsQueryHandler : IRequestHandler<GetJobApplicantsQuery, Pagination<JobApplicantResponse>>
    {
        private readonly IJobApplicantRepository _jobApplicantRepository;

        public GetJobApplicantsQueryHandler(IJobApplicantRepository jobApplicantRepository)
        {
            _jobApplicantRepository = jobApplicantRepository;
        }

        public async Task<Pagination<JobApplicantResponse>> Handle(GetJobApplicantsQuery request, CancellationToken cancellationToken)
        {
            var jobApplicants = await _jobApplicantRepository.GetJobApplicantsAsync(request.JobApplicantSpecParam);

            return AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<JobApplicantResponse>>(jobApplicants);
        }
    }
}
