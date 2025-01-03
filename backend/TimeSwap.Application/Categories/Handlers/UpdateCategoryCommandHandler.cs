﻿using MediatR;
using TimeSwap.Application.Categories.Commands;
using TimeSwap.Application.Exceptions.Categories;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Domain.Interfaces.Repositories;

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
            var industry = await _industryRepository.GetByIdAsync(request.IndustryId);
            if (industry == null)
            {
                throw new IndustryNotFoundException();
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new CategoryNotFoundException();
            }

            category.CategoryName = request.CategoryName;
            category.IndustryId = request.IndustryId;

            await _categoryRepository.UpdateAsync(category);
            return true;
        }
    }
}
