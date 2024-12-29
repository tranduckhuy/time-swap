using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TimeSwap.Application.Dtos.Auth.Requests;
using TimeSwap.Application.Dtos.Auth.Responses;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Application.Interfaces.Services;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Infrastructure.Authentication;
using TimeSwap.Infrastructure.Email;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailSender;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtHandler _jwtHandler;
        private readonly ITokenBlackListService _tokenBlackListService;

        public AuthService(UserManager<ApplicationUser> userManager, IEmailService emailSender,
            ILogger<AuthService> logger, JwtHandler jwtHandler, ITokenBlackListService tokenBlackListService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _jwtHandler = jwtHandler;
            _tokenBlackListService = tokenBlackListService;
        }

        public async Task<StatusCode> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Attempt to register with existing email: {email}.", request.Email);
                throw new EmailAlreadyExistsException();
            }

            var newUser = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Failed to create user. Errors: {errors}", string.Join(", ", errors));
                throw new AppException(StatusCode.ProvidedInformationIsInValid, errors);
            }

            await _userManager.AddToRoleAsync(newUser, nameof(Role.User));

            _ = SendConfirmEmailMessage(request.ClientUrl, newUser);

            return StatusCode.ConfirmationEmailSent;
        }

        private async Task SendConfirmEmailMessage(string clientUrl, ApplicationUser newUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var message = MailMessageHelper.CreateMessage(newUser, token, clientUrl, "Confirm Email", "confirm your email");

            _logger.LogInformation("Sending email to '{email}' to confirm email.", newUser.Email);

            _ = _emailSender.SendEmailAsync(message);
        }

        public async Task<(StatusCode, AuthenticationResponse)> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new UserNotExistsException();

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Attempt to login with unconfirmed email: {email}.", request.Email);
                throw new UserNotConfirmedException();
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogWarning("Attempt to login with invalid password for email: {email}.", request.Email);
                throw new InvalidCredentialsException();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authResponse = await GenerateToken(user, userRoles, true);

            return (StatusCode.RequestProcessedSuccessfully, authResponse);
        }

        private async Task<AuthenticationResponse> GenerateToken(ApplicationUser user, IList<string> roles, bool populateExp)
        {
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user, roles);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = _jwtHandler.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            if (populateExp)
            {
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
            }

            await _userManager.UpdateAsync(user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = _jwtHandler.GetExpiryInSecond()
            };
        }

        public async Task<StatusCode> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new UserNotExistsException();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var message = MailMessageHelper.CreateMessage(user, token, request.ClientUrl, "Reset Password", "reset your password");

            _ = _emailSender.SendEmailAsync(message);

            return StatusCode.ResetPasswordEmailSent;
        }

        public async Task<StatusCode> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new UserNotExistsException();

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Failed to reset password. Errors: {errors}", string.Join(", ", errors));
                throw new AuthException(StatusCode.UserAuthenticationFailed, errors);
            }

            return StatusCode.PasswordResetSuccessful;
        }

        public async Task<StatusCode> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new UserNotExistsException();

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Failed to confirm email. Errors: {errors}", string.Join(", ", errors));
                throw new AuthException(StatusCode.ConfirmEmailTokenInvalidOrExpired, errors);
            }

            return StatusCode.RequestProcessedSuccessfully;
        }

        public async Task<StatusCode> ResendConfirmationEmailAsync(ResendConfirmationEmailRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new UserNotExistsException();

            if (user.EmailConfirmed)
            {
                _logger.LogWarning("Attempt to resend confirmation email for already confirmed email: {email}.", request.Email);
                throw new UserAlreadyConfirmedException();
            }

            _ = SendConfirmEmailMessage(request.ClientUrl, user);

            return StatusCode.ConfirmationEmailSent;
        }

        public async Task<(StatusCode, AuthenticationResponse)> RefreshTokenAsync(RefreshTokenDto request)
        {
            var principal = _jwtHandler.GetPrincipalFromExpiredToken(request.AccessToken);

            var username = principal.Identity?.Name ?? throw new InvalidTokenException();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new InvalidTokenException(["Refresh token is invalid or expired."]);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authResponse = await GenerateToken(user, userRoles, false);

            return (StatusCode.RequestProcessedSuccessfully, authResponse);
        }

        public async Task Logout(string userId, string accessToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.RefreshToken = null!;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);
            }

            var expiry = GetTokenExpiry(accessToken);

            if (expiry > DateTime.UtcNow)
            {
                await _tokenBlackListService.BlacklistTokenAsync(accessToken, expiry);
            }
        }

        private DateTime GetTokenExpiry(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.ValidTo;
        }

    }
}
