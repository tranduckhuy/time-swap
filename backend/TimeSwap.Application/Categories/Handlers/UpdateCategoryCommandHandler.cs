using MediatR;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Application.Exceptions.Categories;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIndustryRepository _industryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IIndustryRepository industryRepository)
        {
            _categoryRepository = categoryRepository;
            _industryRepository = industryRepository;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _ = await _industryRepository.GetByIdAsync(request.IndustryId) ?? throw new IndustryNotFoundException();

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId) ?? throw new CategoryNotFoundException();
            if (request.CategoryId != category.Id)
            {
                if (await _categoryRepository.GetCategoryByNameAsync(request.CategoryName) != null)
                {
                    throw new CategorySameNameException();
                }
            }

            category.CategoryName = request.CategoryName;
            category.IndustryId = request.IndustryId;

            await _categoryRepository.UpdateAsync(category);
            return Unit.Value;
        }
    }
}
