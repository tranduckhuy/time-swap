using MediatR;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Categories.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIndustryRepository _industryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IIndustryRepository industryRepository)
        {
            _categoryRepository = categoryRepository;
            _industryRepository = industryRepository;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.GetByIdAsync(request.IndustryId);
            if (industry == null)
            {
                throw new AppException(StatusCode.IndustryNotFound);
            }

            var category = new Category
            {
                CategoryName = request.CategoryName,
                IndustryId = request.IndustryId
            };

            category = await _categoryRepository.AddAsync(category);
            return category.Id;
        }
    }
}
