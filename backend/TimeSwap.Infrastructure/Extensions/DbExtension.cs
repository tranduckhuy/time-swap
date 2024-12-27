﻿using Microsoft.AspNetCore.Builder;
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

                if (!context.Roles.Any())
                {
                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(userRole);
                }

                if (!context.Users.Any())
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
                if (!context.Locations.Any())
                {
                    var locations = new List<Location>
                    {
                        new Location
                        {
                            Id = new Guid("1b1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b"),
                            Street = "An Phú Thịnh",
                            Ward = "Nhơn Bình",
                            City = "Quy Nhơn",
                            Province = "Bình Định"
                        },

                        new Location
                        {
                            Id = new Guid("2b2b2b2b-2b2b-2b2b-2b2b-2b2b2b2b2b2b"),
                            Street = "Trần Hưng Đạo",
                            Ward = "Đống Đa",
                            City = "Quy Nhơn",
                            Province = "Bình Định"
                        }
                    };

                    await context.Locations.AddRangeAsync(locations);
                }

                if (!context.UserProfiles.Any())
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
                            LocationId = new Guid("1b1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b")
                        },

                        new UserProfile
                        {
                            Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            CurrentSubscription = SubscriptionPlan.Basic,
                            SubscriptionExpiryDate = DateTime.UtcNow.AddMonths(1),
                            Balance = 0,
                            Description = "Tôi là một người dọn dẹp chuyên nghiệp, hãy để tôi giúp bạn dọn dẹp nhà cửa của mình!",
                            AvatarUrl = "https://gravatar.com/images/homepage/avatar-07.png",
                            LocationId = new Guid("2b2b2b2b-2b2b-2b2b-2b2b-2b2b2b2b2b2b")
                        }
                    };

                    await context.UserProfiles.AddRangeAsync(userProfiles);
                }

                if (!context.Categories.Any())
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

                if (!context.Industries.Any())
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

                if (!context.JobPosts.Any())
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
                            DueDate = DateTime.UtcNow.AddDays(7),
                            CategoryId = 1,
                            IndustryId = 1,
                            LocationId = new Guid("1b1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b"),
                        },

                        new JobPost
                        {
                            Id = new Guid("2c2c2c2c-2c2c-2c2c-2c2c-2c2c2c2c2c2c"),
                            UserId = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            Title = "Dọn dẹp nhà cửa",
                            Description = "Cần người dọn dẹp nhà cửa",
                            Fee = 50,
                            DueDate = DateTime.UtcNow.AddDays(3),
                            CategoryId = 2,
                            IndustryId = 2,
                            LocationId = new Guid("2b2b2b2b-2b2b-2b2b-2b2b-2b2b2b2b2b2b"),
                        }
                    };

                    await context.JobPosts.AddRangeAsync(jobPosts);
                }

                if (!context.JobApplicants.Any())
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
                    };

                    await context.JobApplicants.AddRangeAsync(jobApplicants);
                }

                if (!context.Reviews.Any())
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

                if (!context.ReviewsCriteria.Any())
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