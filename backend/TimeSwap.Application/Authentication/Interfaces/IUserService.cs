using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Authentication.Interfaces
{
    public interface IUserService
    {
        Task<(StatusCode, UserResponse)> GetUserProfileAsync(Guid userId);
    }
}
