using Microsoft.Extensions.Logging;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.Categories;
using TimeSwap.Application.Exceptions.Industries;
using TimeSwap.Application.Exceptions.JobPost;
using TimeSwap.Application.Exceptions.Location;
using TimeSwap.Application.Exceptions.User;
using TimeSwap.Application.JobPosts.Commands;
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


        public async Task ValidateCreateJobPostAsync(CreateJobPostCommand request)
        {
            await ValidateAsync(request.CategoryId, request.IndustryId, request.WardId, request.CityId);

            ValidateJobPostCommandDateTime(request.StartDate, request.DueDate);

        }

        private void ValidateJobPostCommandDateTime(DateTime? startDate, DateTime dueDate)
        {
            if (startDate.HasValue && dueDate <= startDate)
            {
                _logger.LogWarning("[JobPostCommand] - DueDate must be greater than StartDate");
                throw new DueDateMustBeGreaterThanStartDateException();
            }

            if (dueDate <= DateTime.UtcNow)
            {
                _logger.LogWarning("[JobPostCommand] - DueDate must be greater than current date");
                throw new DueDateMustBeGreaterThanCurrentDateException();
            }
        }

        public async Task ValidateUpdateJobPostAsync(UpdateJobPostCommand request)
        {
            await ValidateAsync(request.CategoryId, request.IndustryId, request.WardId, request.CityId);

            await ValidateUserAsync(request.UserId, request.Fee);

            ValidateJobPostCommandDateTime(request.StartDate, request.DueDate);
        }

        private async Task ValidateAsync(int categoryId, int industryId, string? wardId, string? cityId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                _logger.LogWarning("[JobPostCommand] - Category with id {CategoryId} not found", categoryId);
                throw new CategoryNotFoundException();
            }

            var industry = await _industryRepository.GetByIdAsync(industryId);
            if (industry == null)
            {
                _logger.LogWarning("[JobPostCommand] - Industry with id {IndustryId} not found", industryId);
                throw new IndustryNotFoundException();
            }

            if (category.IndustryId != industry.Id)
            {
                _logger.LogWarning("[JobPostCommand] - Category with id {CategoryId} does not belong to Industry with id {IndustryId}",
                    categoryId, industryId);
                throw new InvalidCategoryInIndustryException();
            }

            await ValidateWardAndCityAsync(wardId, cityId);
        }

        private async Task ValidateUserAsync(Guid userId, decimal fee)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with id {UserId} not found", userId);
                throw new UserNotExistsException();
            }

            if (fee < 50000)
            {
                _logger.LogWarning("[User:{userId}] on [JobPostCommand] - Fee must be greater than 50,000 VND", userId);
                throw new FeeMustBeGreaterThanFiftyThousandException();
            }

            // Check if user has enough balance to create job post
            if (user.Balance < fee * (0.1m))
            {
                _logger.LogWarning("[User:{userId}] on [JobPostCommand] - User does not have enough balance to create job post", userId);
                throw new UserNotEnoughBalanceException();
            }
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
