using Microsoft.Extensions.Logging;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.Categories;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Exceptions.Location;
using TimeSwap.Application.Exceptions.User;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.JobPosts.Validators
{
    public class JobPostValidatorService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWardRepository _wardRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ILogger<JobPostValidatorService> _logger;

        public JobPostValidatorService(
            ICategoryRepository categoryRepository,
            IIndustryRepository industryRepository,
            IUserRepository userRepository,
            IWardRepository wardRepository,
            ICityRepository cityRepository,
            ILogger<JobPostValidatorService> logger)
        {
            _categoryRepository = categoryRepository;
            _industryRepository = industryRepository;
            _userRepository = userRepository;
            _wardRepository = wardRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task ValidateAsync(CreateJobPostCommand request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                _logger.LogWarning("Category with id {CategoryId} not found", request.CategoryId);
                throw new CategoryNotFoundException();
            }

            var industry = await _industryRepository.GetByIdAsync(request.IndustryId);
            if (industry == null)
            {
                _logger.LogWarning("Industry with id {IndustryId} not found", request.IndustryId);
                throw new IndustryNotFoundException();
            }

            if (category.IndustryId != industry.Id)
            {
                _logger.LogWarning("Category with id {CategoryId} is not valid in Industry with id {IndustryId}", request.CategoryId, request.IndustryId);
                throw new InvalidCategoryInIndustryException();
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User with id {UserId} not found", request.UserId);
                throw new UserNotExistsException();
            }

            // Check if user has enough balance to create job post
            if (user.Balance < request.Fee * (1 + 0.1m))
            {
                _logger.LogWarning("User with id {UserId} does not have enough balance", request.UserId);
                throw new UserNotEnoughBalanceException();
            }

            await ValidateWardAndCityAsync(request.WardId, request.CityId);
        }

        private async Task ValidateWardAndCityAsync(string? wardId, string? cityId)
        {
            if (!string.IsNullOrEmpty(wardId) && string.IsNullOrEmpty(cityId))
            {
                _logger.LogWarning("CityId is required when WardId is provided.");
                throw new CityIdRequireWhenWardIdProvidedException();
            }

            if (string.IsNullOrEmpty(wardId) && !string.IsNullOrEmpty(cityId))
            {
                _logger.LogWarning("WardId is required when CityId is provided.");
                throw new WardIdRequireWhenCityIdProvidedException();
            }

            if (!string.IsNullOrEmpty(wardId) && !string.IsNullOrEmpty(cityId))
            {
                var ward = await _wardRepository.GetByIdAsync(wardId);
                if (ward == null)
                {
                    _logger.LogWarning("Ward with id {WardId} not found", wardId);
                    throw new WardNotFoundException();
                }

                var city = await _cityRepository.GetByIdAsync(cityId);
                if (city == null)
                {
                    _logger.LogWarning("City with id {CityId} not found", cityId);
                    throw new CityNotFoundException();
                }

                var isValidWardInCity = await _wardRepository.ValidateWardInCityAsync(wardId, cityId);
                if (!isValidWardInCity)
                {
                    _logger.LogWarning("Ward with id {WardId} is not valid in City with id {CityId}", wardId, cityId);
                    throw new InvalidWardInCityException();
                }
            }
        }
    }
}
