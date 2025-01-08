using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeSwap.Application.Authentication.Dtos.Requests;
using TimeSwap.Application.Authentication.Dtos.Responses;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Application.Mappings;
using TimeSwap.Auth.Mappings;
using TimeSwap.Auth.Models.Requests;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Auth.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger) : base(logger)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<RegisterRequestDto>(request);

            return await HandleRequestAsync(dto, _authService.RegisterAsync);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<LoginRequestDto>(request);
            return await HandleRequestWithResponseAsync<LoginRequestDto, AuthenticationResponse>
                (dto, _authService.LoginAsync);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<ForgotPasswordRequestDto>(request);

            return await HandleRequestAsync(dto, _authService.ForgotPasswordAsync);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<ResetPasswordRequestDto>(request);
            return await HandleRequestAsync(dto, _authService.ResetPasswordAsync);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<ConfirmEmailRequestDto>(request);
            return await HandleRequestAsync(dto, _authService.ConfirmEmailAsync);
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
        {
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<ResendConfirmationEmailRequestDto>(request);
            return await HandleRequestAsync(dto, _authService.ResendConfirmationEmailAsync);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var dto = AppMapper<AuthMappingProfile>.Mapper.Map<RefreshTokenDto>(request);
            return await HandleRequestWithResponseAsync<RefreshTokenDto, AuthenticationResponse>
                (dto, _authService.RefreshTokenAsync);
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
