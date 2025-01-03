using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.JobPosts.Validators;

namespace TimeSwap.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetJobPostsQuery).Assembly));
            services.AddAutoMapper(typeof(GetJobPostsQuery).Assembly);

            services.AddScoped<JobPostValidatorService>();

            return services;
        }
    }
}
