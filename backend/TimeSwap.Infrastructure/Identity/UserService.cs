using Microsoft.AspNetCore.Identity;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Application.Authentication.User;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Validators;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly ITransactionManager _transactionManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly LocationValidatorService _locationValidatiorService;
        private readonly CategoryIndustryValidatorService _categoryIndustryValidatorService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            LocationValidatorService locationValidatiorService,
            CategoryIndustryValidatorService categoryIndustryValidatorService,
            ITransactionManager transactionManager)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _locationValidatiorService = locationValidatiorService;
            _categoryIndustryValidatorService = categoryIndustryValidatorService;
            _transactionManager = transactionManager;
        }

        public async Task<(StatusCode, UserResponse)> GetUserProfileAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new UserNotExistsException();
            var userProfile = await _userRepository.GetUserProfileAsync(userId) ?? throw new UserNotExistsException();
            var userRoles = await _userManager.GetRolesAsync(user);

            var userResponse = new UserResponse
            {
                Id = userProfile.Id,
                Email = userProfile.Email,
                FullName = userProfile.FullName,
                PhoneNumber = user.PhoneNumber!,
                Role = userRoles.ToList(),
                FullLocation = userProfile.Ward?.FullLocation,
                AvatarUrl = userProfile.AvatarUrl,
                Description = userProfile.Description,
                Balance = userProfile.Balance,
                SubscriptionPlan = userProfile.CurrentSubscription,
                SubscriptionExpiryDate = userProfile.SubscriptionExpiryDate,
                EducationHistory = userProfile.EducationHistory,
                MajorCategory = userProfile.MajorCategory?.CategoryName,
                MajorIndustry = userProfile.MajorIndustry?.IndustryName
            };

            return (StatusCode.RequestProcessedSuccessfully, userResponse);
        }

        public async Task<StatusCode> UpdateUserProfileAsync(UpdateUserProfileRequestDto request)
        {
            await _transactionManager.ExecuteAsync(async () =>
            {
                // Update Identity database
                var user = await _userManager.FindByIdAsync(request.UserId.ToString()) ?? throw new UserNotExistsException();

                if (!string.IsNullOrEmpty(request.FirstName)) user.FirstName = request.FirstName.Trim();
                if (!string.IsNullOrEmpty(request.LastName)) user.LastName = request.LastName.Trim();

                if (!string.IsNullOrEmpty(request.PhoneNumber)) user.PhoneNumber = request.PhoneNumber.Trim();

                var identityUpdateResult = await _userManager.UpdateAsync(user);
                if (!identityUpdateResult.Succeeded)
                {
                    throw new AppException(StatusCode.RequestProcessingFailed, identityUpdateResult.Errors.Select(e => e.Description).ToList());
                }

                // Update Core database
                var userProfile = await _userRepository.GetUserProfileAsync(request.UserId) ?? throw new UserNotExistsException();

                userProfile.FullName = $"{user.FirstName} {user.LastName}";

                if (!string.IsNullOrEmpty(request.CityId) && !string.IsNullOrEmpty(request.WardId))
                {
                    await _locationValidatiorService.ValidateWardAndCityAsync(request.WardId, request.CityId);
                    userProfile.CityId = request.CityId;
                    userProfile.WardId = request.WardId;
                }

                if (!string.IsNullOrEmpty(request.Description)) userProfile.Description = request.Description;
                if (!string.IsNullOrEmpty(request.AvatarUrl)) userProfile.AvatarUrl = request.AvatarUrl;
                if (request.EducationHistory?.Count > 0) userProfile.EducationHistory = request.EducationHistory;

                if (request.MajorIndustryId != 0 && request.MajorCategoryId != 0)
                {
                    await _categoryIndustryValidatorService.ValidateCategoryAndIndustryAsync(request.MajorCategoryId, request.MajorIndustryId);
                    userProfile.MajorCategoryId = request.MajorCategoryId;
                    userProfile.MajorIndustryId = request.MajorIndustryId;
                }

                await _userRepository.UpdateAsync(userProfile);
            });

            return StatusCode.RequestProcessedSuccessfully;
        }
    }
}
