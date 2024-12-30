using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TimeSwap.Application.Interfaces.Services;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Infrastructure.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            // Bypass if the endpoint has [AllowAnonymous] attribute
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var tokenBlacklistService = scope.ServiceProvider.GetRequiredService<ITokenBlackListService>();

                    if (await tokenBlacklistService.IsTokenBlacklistedAsync(token))
                    {
                        var statusCode = Shared.Constants.StatusCode.TokenIsBlacklisted;
                        var apiResponse = new ApiResponse<object>
                        {
                            StatusCode = (int)statusCode,
                            Message = ResponseMessages.GetMessage(statusCode)
                        };

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse));
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
