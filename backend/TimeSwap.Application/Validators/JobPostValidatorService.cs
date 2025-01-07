using Microsoft.Extensions.Logging;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.JobPost;
using TimeSwap.Application.Exceptions.User;
using TimeSwap.Application.JobPosts.Commands;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Validators
{
    public class JobPostValidatorService
    {
        private readonly IUserRepository _userRepository;
        private readonly LocationValidatorService _locationValidatiorService;
        private readonly CategoryIndustryValidatorService _categoryIndustryValidatorService;
        private readonly ILogger<JobPostValidatorService> _logger;

        public JobPostValidatorService(
            IUserRepository userRepository,
            ILogger<JobPostValidatorService> logger,
            LocationValidatorService locationValidatiorService,
            CategoryIndustryValidatorService categoryIndustryValidatorService)
        {
            _userRepository = userRepository;
            _locationValidatiorService = locationValidatiorService;
            _logger = logger;
            _categoryIndustryValidatorService = categoryIndustryValidatorService;
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
            await _categoryIndustryValidatorService.ValidateCategoryAndIndustryAsync(categoryId, industryId);

            await _locationValidatiorService.ValidateWardAndCityAsync(wardId, cityId);
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
            if (user.Balance < fee * 0.1m)
            {
                _logger.LogWarning("[User:{userId}] on [JobPostCommand] - User does not have enough balance to create job post", userId);
                throw new UserNotEnoughBalanceException();
            }
        }
    }
}
