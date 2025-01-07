using TimeSwap.Application.Authentication.User;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Authentication.Interfaces
{
    public interface IUserService
    {
        Task<(StatusCode, UserResponse)> GetUserProfileAsync(Guid userId);
        Task<StatusCode> UpdateUserProfileAsync(UpdateUserProfileRequestDto request);
    }
}
