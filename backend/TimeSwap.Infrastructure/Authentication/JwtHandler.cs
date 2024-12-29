using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TimeSwap.Application.Exceptions.Auth;
using TimeSwap.Domain.Exceptions;
using TimeSwap.Infrastructure.Identity;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Authentication
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly ILogger<JwtHandler> _logger;

        public JwtHandler(IConfiguration configuration, ILogger<JwtHandler> logger)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _logger = logger;
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings["SecretKey"] ?? throw new InvalidDataException("SecretKey is missing in appsettings.json"));
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public List<Claim> GetClaims(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["ValidIssuer"],
                audience: _jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(GetExpiryInSecond()),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        public int GetExpiryInSecond()
        {
            return Convert.ToInt32(_jwtSettings["ExpiryInSecond"]);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["SecretKey"]!)),
                ValidateLifetime = false,
                ValidIssuer = _jwtSettings["ValidIssuer"],
                ValidAudience = _jwtSettings["ValidAudience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new InvalidTokenException(["The token is invalid. The expected signing algorithm is HMACSHA256, but the token's algorithm does not match."]);
                }
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while validating token.");
                throw new InvalidTokenException([ex.Message]);
            }
        }
    }
}
