using MediatR;
using TimeSwap.Application.Industries.Commands;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
{
    public class CreateIndustryCommandHandler : IRequestHandler<CreateIndustryCommand, int>
    {
        private readonly IIndustryRepository _industryRepository;

        public CreateIndustryCommandHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        public async Task<int> Handle(CreateIndustryCommand request, CancellationToken cancellationToken)
        {

            var industry = new Industry
            {
                IndustryName = request.IndustryName
            };

            industry = await _industryRepository.AddAsync(industry);
            return industry.Id;
        }
    }
}
