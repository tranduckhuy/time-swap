using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeSwap.Application.Authentication.Dtos.Requests;
using TimeSwap.Application.Authentication.Dtos.Responses;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Auth.Models.Requests;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return await HandleRequestAsync<RegisterRequest, RegisterRequestDto>(request, _authService.RegisterAsync);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return await HandleRequestWithResponseAsync<LoginRequest, LoginRequestDto, AuthenticationResponse>
                (request, _authService.LoginAsync);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            return await HandleRequestAsync<ForgotPasswordRequest, ForgotPasswordRequestDto>
                (request, _authService.ForgotPasswordAsync);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            return await HandleRequestAsync<ResetPasswordRequest, ResetPasswordRequestDto>
                (request, _authService.ResetPasswordAsync);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            return await HandleRequestAsync<ConfirmEmailRequest, ConfirmEmailRequestDto>
                (request, _authService.ConfirmEmailAsync);
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
        {
            return await HandleRequestAsync<ResendConfirmationEmailRequest, ResendConfirmationEmailRequestDto>
                (request, _authService.ResendConfirmationEmailAsync);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            return await HandleRequestWithResponseAsync<RefreshTokenRequest, RefreshTokenDto, AuthenticationResponse>
                (request, _authService.RefreshTokenAsync);
        }

        [HttpDelete("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId != null) {

                await _authService.Logout(userId, token);
                
                var statusCode = Shared.Constants.StatusCode.RequestProcessedSuccessfully;

                return Ok(new ApiResponse<object>
                {
                    StatusCode = (int)statusCode,
                    Message = ResponseMessages.GetMessage(statusCode)
                });
            }

            return BadRequest(new ApiResponse<object>
            {
                StatusCode = (int)Shared.Constants.StatusCode.UserNotExists,
                Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.UserNotExists)
            });
        }
    }
}
