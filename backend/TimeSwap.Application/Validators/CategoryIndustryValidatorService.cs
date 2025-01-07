using Microsoft.Extensions.Logging;
using TimeSwap.Application.Exceptions.Categories;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Exceptions.Location;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Validators
{
    public class CategoryIndustryValidatorService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly ILogger<CategoryIndustryValidatorService> _logger;


        public CategoryIndustryValidatorService(
            ICategoryRepository categoryRepository, 
            IIndustryRepository industryRepository, 
            ILogger<CategoryIndustryValidatorService> logger)
        {
            _categoryRepository = categoryRepository;
            _industryRepository = industryRepository;
            _logger = logger;
        }

        public async Task ValidateCategoryAndIndustryAsync(int categoryId, int industryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                _logger.LogWarning("[CategoryIndustryValidatorService] - Category with id {CategoryId} not found", categoryId);
                throw new CategoryNotFoundException();
            }

            var industry = await _industryRepository.GetByIdAsync(industryId);
            if (industry == null)
            {
                _logger.LogWarning("[CategoryIndustryValidatorService] - Industry with id {IndustryId} not found", industryId);
                throw new IndustryNotFoundException();
            }

            if (category.IndustryId != industry.Id)
            {
                _logger.LogWarning("[CategoryIndustryValidatorService] - Category with id {CategoryId} is not in Industry with id {IndustryId}",
                    categoryId, industryId);
                throw new InvalidCategoryInIndustryException();
            }
        }
    }
}
