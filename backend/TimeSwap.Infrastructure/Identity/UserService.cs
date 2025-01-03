using Microsoft.AspNetCore.Identity;
using TimeSwap.Application.Authentication;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
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
                SubscriptionExpiryDate = userProfile.SubscriptionExpiryDate
            };

            return (StatusCode.RequestProcessedSuccessfully, userResponse);
        }
    }
}
