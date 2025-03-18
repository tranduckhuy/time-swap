using TimeSwap.Application.Authentication.Dtos.Requests;
using TimeSwap.Application.Authentication.Dtos.Responses;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Authentication.Interfaces
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
        Task<StatusCode> ChangePasswordAsync(ChangePasswordRequestDto dto);
        Task<StatusCode> LockUnlockAccountAsync(LockUnlockAccountRequestDto request);
    }
}
