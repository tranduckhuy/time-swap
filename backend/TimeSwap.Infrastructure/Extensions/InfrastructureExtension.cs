using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TimeSwap.Application.Interfaces.Services;
using TimeSwap.Domain.Interfaces.Repositories;
using TimeSwap.Infrastructure.Authentication;
using TimeSwap.Infrastructure.Configurations;
using TimeSwap.Infrastructure.Email;
using TimeSwap.Infrastructure.Identity;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Infrastructure.Persistence.Repositories;

namespace TimeSwap.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:ConnectionString"];
                options.InstanceName = configuration["Redis:InstanceName"];
            });

            services.AddScoped<IAuthService, AuthService>();

            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig ?? throw new InvalidDataException("EmailConfiguration is missing in appsettings.json"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped(typeof(IAsyncRepository<,>), typeof(RepositoryBase<,>));
            services.AddScoped<ITokenBlackListService, TokenBlackListService>();
            services.AddSingleton<JwtHandler>();

            return services;
        }

        public static IServiceCollection AddApplicationJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = configuration["JWTSettings:ValidAudience"],
                    ValidIssuer = configuration["JWTSettings:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWTSettings:SecretKey"]
                        ?? throw new InvalidDataException("JWTSettings:SecretKey is missing in appsettings.json")))
                };
            });
            return services;
        }

        public static IdentityBuilder AddApplicationIdentity<TUser>(this IServiceCollection services) where TUser : class
        {
            return services.AddIdentity<TUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<UserIdentityDbContext>()
            .AddDefaultTokenProviders();
        }

    }
}
