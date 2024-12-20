using Microsoft.Extensions.DependencyInjection;
using TimeSwap.Domain.Repositories;
using TimeSwap.Infrastructure.Repositories;

namespace TimeSwap.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            return services;
        }
    }
}
