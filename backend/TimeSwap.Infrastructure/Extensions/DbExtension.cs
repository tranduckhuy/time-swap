using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeSwap.Domain.Entities;
using TimeSwap.Infrastructure.Identity;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Shared.Constants;


namespace TimeSwap.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidDataException("The connection string is missing in the configuration. Please provide a valid connection string.");

            services.AddDbContext<TContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            return services;
        }

        public static async Task<IApplicationBuilder> SeedAuthDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<UserIdentityDbContext>();

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // TODO: Remove this block of code before deploying to production
            //await context.Database.EnsureDeletedAsync();

            if (await context.Database.EnsureCreatedAsync())
            {
                var adminRole = new IdentityRole(nameof(Role.Admin));
                var userRole = new IdentityRole(nameof(Role.User));

                if (!await context.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(userRole);
                }

                if (!await context.Users.AnyAsync())
                {
                    var adminUser = new ApplicationUser
                    {
                        Id = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a").ToString(),
                        Email = "huytde.dev@gmail.com",
                        UserName = "huytde.dev@gmail.com",
                        EmailConfirmed = true,
                        FirstName = "Huy",
                        LastName = "Tran",
                        PhoneNumber = "0838683869"
                    };

                    await userManager.CreateAsync(adminUser, "Admin11@");
                    await userManager.AddToRoleAsync(adminUser, nameof(Role.Admin));

                    var user = new ApplicationUser
                    {
                        Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a").ToString(),
                        Email = "user@gmail.com",
                        UserName = "user@gmail.com",
                        EmailConfirmed = true,
                        FirstName = "User",
                        LastName = "Test",
                        PhoneNumber = "0838683868"
                    };

                    await userManager.CreateAsync(user, "User111@");
                    await userManager.AddToRoleAsync(user, nameof(Role.User));
                }
            }

            return app;
        }

        public static async Task<IApplicationBuilder> SeedCoreDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            // TODO: Remove this block of code before deploying to production
            //await context.Database.EnsureDeletedAsync();

            if (await context.Database.EnsureCreatedAsync())
            {
                if (!await context.UserProfiles.AnyAsync())
                {
                    var userProfiles = new List<UserProfile>
                    {
                        new UserProfile
                        {
                            Id = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            CurrentSubscription = SubscriptionPlan.Premium,
                            SubscriptionExpiryDate = DateTime.UtcNow.AddMonths(1),
                            Balance = 9999,
                            Description = "Tôi là một lập trình viên, tôi yêu thích công việc của mình. Hãy để tôi giúp bạn!",
                            AvatarUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50",
                            LocationIds = ["52", "540", "21550"]
                        },

                        new UserProfile
                        {
                            Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            CurrentSubscription = SubscriptionPlan.Basic,
                            SubscriptionExpiryDate = DateTime.UtcNow.AddMonths(1),
                            Balance = 0,
                            Description = "Tôi là một người dọn dẹp chuyên nghiệp, hãy để tôi giúp bạn dọn dẹp nhà cửa của mình!",
                            AvatarUrl = "https://gravatar.com/images/homepage/avatar-07.png",
                            LocationIds = ["52", "540", "21556"]
                        }
                    };

                    await context.UserProfiles.AddRangeAsync(userProfiles);
                }

                if (!await context.Categories.AnyAsync())
                {
                    var categories = new List<Category>
                    {
                        new Category
                        {
                            Id = 1,
                            CategoryName = "Code hộ"
                        },
                        new Category
                        {
                            Id = 2,
                            CategoryName = "Dọn dẹp"
                        }
                    };

                    await context.Categories.AddRangeAsync(categories);
                }

                if (!await context.Industries.AnyAsync())
                {
                    var industries = new List<Industry>
                    {
                        new Industry
                        {
                            Id = 1,
                            IndustryName = "Công nghệ thông tin"
                        },
                        new Industry
                        {
                            Id = 2,
                            IndustryName = "Dịch vụ"
                        }
                    };

                    await context.Industries.AddRangeAsync(industries);
                }

                if (!await context.JobPosts.AnyAsync())
                {
                    var jobPosts = new List<JobPost>
                    {
                        new JobPost
                        {
                            Id = new Guid("1c1c1c1c-1c1c-1c1c-1c1c-1c1c1c1c1c1c"),
                            UserId = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            Title = "Code hộ",
                            Description = "Cần code hộ 1 trang web",
                            Fee = 100,
                            AssignedTo = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            DueDate = DateTime.UtcNow.AddDays(7),
                            CategoryId = 1,
                            IndustryId = 1,
                            LocationIds = ["52", "540", "21550"]
                        },

                        new JobPost
                        {
                            Id = new Guid("2c2c2c2c-2c2c-2c2c-2c2c-2c2c2c2c2c2c"),
                            UserId = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            Title = "Code hộ",
                            Description = "Cần code hộ backend cho 1 ứng dụng di động",
                            Fee = 200,
                            DueDate = DateTime.UtcNow.AddDays(30),
                            CategoryId = 1,
                            IndustryId = 1,
                            LocationIds = ["52", "540", "21550"]
                        },

                        new JobPost
                        {
                            Id = new Guid("3c3c3c3c-3c3c-3c3c-3c3c-3c3c3c3c3c3c"),
                            UserId = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            Title = "Dọn dẹp nhà cửa",
                            Description = "Cần người dọn dẹp nhà cửa",
                            Fee = 50,
                            AssignedTo = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            DueDate = DateTime.UtcNow.AddDays(3),
                            CategoryId = 2,
                            IndustryId = 2,
                            LocationIds = ["52", "540", "21556"]
                        }
                    };

                    await context.JobPosts.AddRangeAsync(jobPosts);
                }

                if (!await context.JobApplicants.AnyAsync())
                {
                    var jobApplicants = new List<JobApplicant>
                    {
                        new JobApplicant
                        {
                            Id = new Guid("1d1d1d1d-1d1d-1d1d-1d1d-1d1d1d1d1d1d"),
                            JobPostId = new Guid("1c1c1c1c-1c1c-1c1c-1c1c-1c1c1c1c1c1c"),
                            UserAppliedId = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            AppliedAt = DateTime.UtcNow
                        },

                        new JobApplicant
                        {
                            Id = new Guid("2d2d2d2d-2d2d-2d2d-2d2d-2d2d2d2d2d2d"),
                            JobPostId = new Guid("2c2c2c2c-2c2c-2c2c-2c2c-2c2c2c2c2c2c"),
                            UserAppliedId = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            AppliedAt = DateTime.UtcNow
                        },
                    };

                    await context.JobApplicants.AddRangeAsync(jobApplicants);
                }

                if (!await context.Reviews.AnyAsync())
                {
                    var reviews = new List<Review>
                    {
                        new Review
                        {
                            Id = new Guid("1e1e1e1e-1e1e-1e1e-1e1e-1e1e1e1e1e1e"),
                            JobPostId = new Guid("1c1c1c1c-1c1c-1c1c-1c1c-1c1c1c1c1c1c"),
                            ReviewerId = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            RevieweeId = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            Comment = "Code rất tốt, cảm ơn bạn!"
                        },

                        new Review
                        {
                            Id = new Guid("2e2e2e2e-2e2e-2e2e-2e2e-2e2e2e2e2e2e"),
                            JobPostId = new Guid("1c1c1c1c-1c1c-1c1c-1c1c-1c1c1c1c1c1c"),
                            ReviewerId = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            RevieweeId = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            Comment = "Rất uy tín, đảm bảo chất lượng!"
                        }
                    };
                    await context.Reviews.AddRangeAsync(reviews);
                }

                if (!await context.ReviewsCriteria.AnyAsync())
                {
                    var reviewCriterias = new List<ReviewCriteria>
                    {
                        new ReviewCriteria
                        {
                            Id = new Guid("1f1f1f1f-1f1f-1f1f-1f1f-1f1f1f1f1f1f"),
                            ReviewId = new Guid("1e1e1e1e-1e1e-1e1e-1e1e-1e1e1e1e1e1e"),
                            CriteriaName = ReviewCriteriaType.Punctuality,
                            Rating = 5
                        },

                        new ReviewCriteria
                        {
                            Id = new Guid("2f2f2f2f-2f2f-2f2f-2f2f-2f2f2f2f2f2f"),
                            ReviewId = new Guid("1e1e1e1e-1e1e-1e1e-1e1e-1e1e1e1e1e1e"),
                            CriteriaName = ReviewCriteriaType.QualityOfWork,
                            Rating = 5
                        },

                        new ReviewCriteria
                        {
                            Id = new Guid("3f3f3f3f-3f3f-3f3f-3f3f-3f3f3f3f3f3f"),
                            ReviewId = new Guid("1e1e1e1e-1e1e-1e1e-1e1e-1e1e1e1e1e1e"),
                            CriteriaName = ReviewCriteriaType.Professionalism,
                            Rating = 4
                        }
                    };
                    await context.ReviewsCriteria.AddRangeAsync(reviewCriterias);
                }

                await context.SaveChangesAsync();
            }
            return app;
        }
    }
}