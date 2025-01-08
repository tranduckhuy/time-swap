using MediatR;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Application.Exceptions.Categories;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

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
            _ = await _industryRepository.GetByIdAsync(request.IndustryId) ?? throw new IndustryNotFoundException();

            if (await _categoryRepository.GetCategoryByNameAsync(request.CategoryName) != null)
            {
                throw new CategorySameNameException();
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
