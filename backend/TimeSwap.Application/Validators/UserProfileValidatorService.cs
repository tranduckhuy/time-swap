using Microsoft.Extensions.Logging;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.User;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Validators
{
    public class UserProfileValidatorService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserProfileValidatorService> _logger;
        public UserProfileValidatorService(
            IUserRepository userProfileRepository,
            ILogger<UserProfileValidatorService> logger)
        {
            _userRepository = userProfileRepository;
            _logger = logger;
        }
        public async Task ValidateUserProfileAsync(Guid userId)
        {
            var userProfile = await _userRepository.GetByIdAsync(userId);
            if (userProfile == null)
            {
                _logger.LogWarning("[UserProfileValidatorService] - User profile with user id {UserId} not found", userId);
                throw new UserNotExistsException();
            }

            if (string.IsNullOrEmpty(userProfile.CityId) || string.IsNullOrEmpty(userProfile.WardId) || userProfile.EducationHistory == null
                || string.IsNullOrEmpty(userProfile.Description) || userProfile.MajorCategoryId == null || userProfile.MajorIndustryId == null)
            {
                _logger.LogWarning("[UserProfileValidatorService] - User profile with user id {UserId} is not completed", userId);
                throw new UserProfileNotCompletedException();
            }
        }
    }
}
