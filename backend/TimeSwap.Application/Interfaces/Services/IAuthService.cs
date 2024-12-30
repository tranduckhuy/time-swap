
using TimeSwap.Application.Dtos.Auth.Requests;
using TimeSwap.Application.Dtos.Auth.Responses;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<StatusCode> RegisterAsync(RegisterRequestDto request);
        Task<(StatusCode, AuthenticationResponse)> LoginAsync(LoginRequestDto request);
        Task<StatusCode> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<StatusCode> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<StatusCode> ConfirmEmailAsync(ConfirmEmailRequestDto confirmEmailRequestDto);
        Task<StatusCode> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto resendConfirmationEmailRequestDto);
        Task<(StatusCode, AuthenticationResponse)> RefreshTokenAsync(RefreshTokenDto request);
        Task Logout(string userId, string accessToken);
        Task<StatusCode> AddClaimAsync(string userId, string claimType, string claimValue);
        Task<StatusCode> RemoveClaimAsync(string userId, string claimType, string claimValue);
    }
}
