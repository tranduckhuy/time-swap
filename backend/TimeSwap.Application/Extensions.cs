using Microsoft.Extensions.DependencyInjection;

namespace TimeSwap.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
