using MediatR;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Industries.Commands;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Industries.Handlers
{
    public class UpdateIndustryHandler : IRequestHandler<UpdateIndustryCommand, bool>
    {
        private readonly IIndustryRepository _industryRepository;

        public UpdateIndustryHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        public async Task<bool> Handle(UpdateIndustryCommand request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.GetByIdAsync(request.IndustryId);
            if (industry == null)
            {
                throw new IndustryNotFoundException();
            }

            if (await _industryRepository.GetByNameAsync(request.IndustryName) != null)
            {
                throw new IndustrySameNameException();
            }

            industry.IndustryName = request.IndustryName;

            await _industryRepository.UpdateAsync(industry);
            return true;
        }
    }
}
