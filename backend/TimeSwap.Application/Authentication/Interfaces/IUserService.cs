using TimeSwap.Application.Authentication.User;
using TimeSwap.Domain.Specs;
using TimeSwap.Domain.Specs.User;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Authentication.Interfaces
{
    public interface IUserService
    {
        Task<(StatusCode, UserResponse)> GetUserProfileAsync(Guid userId);
        Task<StatusCode> UpdateSubscriptionAsync(UpdateSubscriptionRequestDto dto);
        Task<StatusCode> UpdateUserProfileAsync(UpdateUserProfileRequestDto request);
        Task<StatusCode> AddUserProfileAsync(AddUserProfileRequestDto request);
        Task<(StatusCode, Pagination<UserResponse>)> GetUserListAsync(UserSpecParam request);
    }
}
