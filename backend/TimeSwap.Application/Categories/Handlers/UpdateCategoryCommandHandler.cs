using MediatR;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIndustryRepository _industryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IIndustryRepository industryRepository)
        {
            _categoryRepository = categoryRepository;
            _industryRepository = industryRepository;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var industry = await _industryRepository.GetByIdAsync(request.IndustryId);
                if (industry == null)
                {
                    throw new AppException(StatusCode.IndustryNotFound);
                }

                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    throw new AppException(StatusCode.CategoryNotFound);
                }

                category.CategoryName = request.CategoryName;
                category.IndustryId = request.IndustryId;

                await _categoryRepository.UpdateAsync(category);
                return true;
            }
            catch (AppException appEx)
            {
                throw new AppException(appEx.StatusCode, appEx.Errors);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the category: {ex.Message}", ex);
            }
        }
    }
}
