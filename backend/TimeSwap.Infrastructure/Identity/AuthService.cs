using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimeSwap.Application.Authentication.Dtos.Requests;
using TimeSwap.Application.Authentication.Dtos.Responses;
using TimeSwap.Application.Authentication.Interfaces;
using TimeSwap.Application.Email;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Domain.Entities;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Authentication;
using TimeSwap.Infrastructure.Email;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtHandler _jwtHandler;
        private readonly ITokenBlackListService _tokenBlackListService;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionManager _transactionManager;

        public AuthService(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<AuthService> logger,
            JwtHandler jwtHandler, ITokenBlackListService tokenBlackListService, IUserRepository userRepository, ITransactionManager transactionManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _jwtHandler = jwtHandler;
            _tokenBlackListService = tokenBlackListService;
            _userRepository = userRepository;
            _transactionManager = transactionManager;
        }

        public async Task<StatusCode> RegisterAsync(RegisterRequestDto request)
        {
            await _transactionManager.ExecuteAsync(async () =>
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Attempt to register with existing email: {email}.", request.Email);
                    throw new EmailAlreadyExistsException();
                }

                var userId = Guid.NewGuid();

                var newUser = new ApplicationUser
                {
                    Id = userId.ToString(),
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
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

                // Update user profile in core database
                await _userRepository.AddAsync(
                    new UserProfile
                    {
                        Id = userId,
                        Email = newUser.Email,
                        FullName = $"{newUser.FirstName} {newUser.LastName}"
                    }
                );

                _logger.LogInformation("Synced user profile with core database for user: {email}.", request.Email);

                _ = SendConfirmEmailMessage(request.ClientUrl, newUser);
            });

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

            var userClaims = await _userManager.GetClaimsAsync(user);

            var authResponse = await GenerateToken(user, userRoles, userClaims, true);

            return (StatusCode.RequestProcessedSuccessfully, authResponse);
        }

        private async Task<AuthenticationResponse> GenerateToken(ApplicationUser user, IList<string> roles, IList<Claim> userClaims, bool populateExp)
        {
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = TokenHelper.GetClaims(user, roles, userClaims);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = TokenHelper.GenerateRefreshToken();

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

            var userClaims = await _userManager.GetClaimsAsync(user);

            var authResponse = await GenerateToken(user, userRoles, userClaims, false);

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

            var expiry = TokenHelper.GetTokenExpiry(accessToken);

            if (expiry > DateTime.UtcNow)
            {
                await _tokenBlackListService.BlacklistTokenAsync(accessToken, expiry);
            }
        }

        public Task<StatusCode> AddClaimAsync(string userId, string claimType, string claimValue)
        {
            throw new NotImplementedException();
        }

        public Task<StatusCode> RemoveClaimAsync(string userId, string claimType, string claimValue)
        {
            throw new NotImplementedException();
        }
    }
}
