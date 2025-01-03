using MediatR;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Industries.Commands;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
{
    public class UpdateIndustryCommandHandler : IRequestHandler<UpdateIndustryCommand, Unit>
    {
        private readonly IIndustryRepository _industryRepository;

        public UpdateIndustryCommandHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        public async Task<Unit> Handle(UpdateIndustryCommand request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.GetByIdAsync(request.IndustryId) ?? throw new IndustryNotFoundException();
            
            if (await _industryRepository.GetByNameAsync(request.IndustryName) != null)
            {
                throw new IndustrySameNameException();
            }

            industry.IndustryName = request.IndustryName;

            await _industryRepository.UpdateAsync(industry);

            return Unit.Value;
        }
    }
}
