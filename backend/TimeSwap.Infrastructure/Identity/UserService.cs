using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Application.Authentication.User;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Exceptions.User;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Validators;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.User;
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

            var userResponse = AppMapper<CoreMappingProfile>.Mapper.Map<UserResponse>(userProfile);
            userResponse.FirstName = user.FirstName;
            userResponse.LastName = user.LastName;
            userResponse.PhoneNumber = user.PhoneNumber!;
            userResponse.Role = userRoles.ToList();

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

                userProfile.ModifiedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(userProfile);
            });

            return StatusCode.RequestProcessedSuccessfully;
        }


        public async Task<StatusCode> UpdateSubscriptionAsync(UpdateSubscriptionRequestDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString()) ?? throw new UserNotExistsException();
            var userProfile = await _userRepository.GetUserProfileAsync(dto.UserId) ?? throw new UserNotExistsException();

            // Swtich case price plan if # basic subscription plan
            if (dto.SubscriptionPlan != SubscriptionPlan.Basic)
            {
                var pricePlan = dto.SubscriptionPlan switch
                {
                    // Standard plan 49,000 VND
                    SubscriptionPlan.Standard => 49000,

                    // Premium plan 99,000 VND
                    SubscriptionPlan.Premium => 99000,
                    _ => throw new AppException(StatusCode.RequestProcessingFailed, ["Invalid subscription plan"])
                };

                if (userProfile.Balance < pricePlan)
                {
                    throw new UserNotEnoughBalanceException();
                }

                userProfile.Balance -= pricePlan;
                userProfile.SubscriptionExpiryDate = DateTime.UtcNow.AddMonths(1);


                var subscriptionExpiryClaim = new Claim("SubscriptionExpiryDate", userProfile.SubscriptionExpiryDate.ToString()!);
                await AddOrReplaceClaimAsync(user, subscriptionExpiryClaim);
            }
            else
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var claimToRemove = claims.FirstOrDefault(c => c.Type == "SubscriptionExpiryDate");
                if (claimToRemove != null)
                {
                    await _userManager.RemoveClaimAsync(user, claimToRemove);
                }

                userProfile.SubscriptionExpiryDate = DateTime.MaxValue;
            }

            userProfile.CurrentSubscription = dto.SubscriptionPlan;
            var subscriptionClaim = new Claim("SubscriptionPlan", dto.SubscriptionPlan.ToString());

            await AddOrReplaceClaimAsync(user, subscriptionClaim);

            userProfile.ModifiedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(userProfile);
            return StatusCode.RequestProcessedSuccessfully;
        }

        private async Task AddOrReplaceClaimAsync(ApplicationUser user, Claim newClaim)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var existingClaim = claims.FirstOrDefault(c => c.Type == newClaim.Type);

            if (existingClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, existingClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }
        }

        public async Task<StatusCode> AddUserProfileAsync(AddUserProfileRequestDto request)
        {
            var user = new UserProfile
            {
                Id = request.UserId,
                Email = request.Email,
                FullName = request.FullName
            };

            await _userRepository.AddAsync(user);

            return StatusCode.RequestProcessedSuccessfully;
        }

        public async Task<(StatusCode, Pagination<UserResponse>)> GetUserListAsync(UserSpecParam request)
        {
            var users = await _userRepository.GetUserWithSpecAsync(request);

            var userResponse = AppMapper<CoreMappingProfile>.Mapper.Map<Pagination<UserResponse>>(users);

            foreach (var user in userResponse.Data)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
                user.IsLocked = identityUser?.LockoutEnd > DateTime.UtcNow;
            }

            return (StatusCode.RequestProcessedSuccessfully, userResponse);
        }
    }
}
