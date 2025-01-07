using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeSwap.Application.JobPosts.Queries;
using TimeSwap.Application.Validators;

namespace TimeSwap.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetJobPostsQuery).Assembly));
            services.AddAutoMapper(typeof(GetJobPostsQuery).Assembly);

            services.AddModelValidator();

            return services;
        }

        public static IServiceCollection AddModelValidator(this IServiceCollection services)
        {
            services.AddScoped<CategoryIndustryValidatorService>();
            services.AddScoped<JobPostValidatorService>();
            services.AddScoped<LocationValidatorService>();

            return services;
        }
    }
}
