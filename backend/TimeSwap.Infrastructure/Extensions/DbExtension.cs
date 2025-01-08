using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TimeSwap.Domain.Entities;
using TimeSwap.Infrastructure.Identity;
using TimeSwap.Infrastructure.Persistence.DbContexts;
using TimeSwap.Shared.Constants;


namespace TimeSwap.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            return services;
        }

        public static async Task LoadLocationDataAsync(this WebApplication app)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Persistence", "Location", "data.json");

            var jsonData = await File.ReadAllTextAsync(path);

            var cities = JsonConvert.DeserializeObject<List<City>>(jsonData)
                ?? throw new InvalidDataException("The location data is missing. Please provide a valid location data.");

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            if (await context.Database.EnsureCreatedAsync() && !await context.Cities.AnyAsync())
            {
                foreach (var cityData in cities)
                {
                    var city = new City
                    {
                        Id = cityData.Id,
                        Name = cityData.Name,
                        Img = cityData.Img,
                    };

                    context.Cities.Add(city);
                    await context.SaveChangesAsync();

                    foreach (var districtData in cityData.Districts)
                    {
                        var district = new District
                        {
                            Id = districtData.Id,
                            Name = districtData.Name,
                            CityId = city.Id,
                        };

                        context.Districts.Add(district);
                        await context.SaveChangesAsync();

                        foreach (var wardData in districtData.Wards)
                        {
                            var ward = new Ward
                            {
                                Id = wardData.Id,
                                Name = wardData.Name,
                                DistrictId = district.Id,
                                Level = wardData.Level,
                                FullLocation = (wardData.Id == district.Id) ? wardData.FullLocation
                                                    : ($"{wardData.Level} {wardData.Name}, {districtData.Name}, {cityData.Name}")
                            };

                            context.Wards.Add(ward);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task<IApplicationBuilder> SeedAuthDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<UserIdentityDbContext>();

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

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
                    var huyAdmin = new ApplicationUser
                    {
                        Id = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a").ToString(),
                        Email = "huytde.dev@gmail.com",
                        UserName = "huytde.dev@gmail.com",
                        EmailConfirmed = true,
                        FirstName = "Huy",
                        LastName = "Tran Duc",
                        PhoneNumber = "0838683869"
                    };

                    await userManager.CreateAsync(huyAdmin, "Admin11@");
                    await userManager.AddToRoleAsync(huyAdmin, nameof(Role.Admin));

                    var quyAdmin = new ApplicationUser
                    {
                        Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a").ToString(),
                        Email = "quynxqe170239@fpt.edu.vn",
                        UserName = "quynxqe170239@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Quy",
                        LastName = "Nguyen Xuan",
                        PhoneNumber = "0838683868"
                    };

                    await userManager.CreateAsync(quyAdmin, "User111@");
                    await userManager.AddToRoleAsync(quyAdmin, nameof(Role.Admin));

                    var sangAdmin = new ApplicationUser
                    {
                        Id = new Guid("3a3a3a3a-3a3a-3a3a-3a3a-3a3a3a3a3a3a").ToString(),
                        Email = "sangtnqe170193@fpt.edu.vn",
                        UserName = "sangtnqe170193@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Sang",
                        LastName = "Tran Ngoc",
                        PhoneNumber = "0838683866"
                    };

                    await userManager.CreateAsync(sangAdmin, "User111@");
                    await userManager.AddToRoleAsync(sangAdmin, nameof(Role.Admin));

                    var huyAdmin2 = new ApplicationUser
                    {
                        Id = new Guid("4a4a4a4a-4a4a-4a4a-4a4a-4a4a4a4a4a4a").ToString(),
                        Email = "huydt170135@fpt.edu.vn",
                        UserName = "huydt170135@fpt.edu.vn",
                        EmailConfirmed = true,
                        FirstName = "Huy",
                        LastName = "Dinh Trong",
                        PhoneNumber = "0838683865"
                    };

                    await userManager.CreateAsync(huyAdmin2, "User111@");
                    await userManager.AddToRoleAsync(huyAdmin2, nameof(Role.Admin));
                }
            }

            return app;
        }

        public static async Task<IApplicationBuilder> SeedCoreDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

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

            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                    {
                        new Category
                        {
                            Id = 1,
                            CategoryName = "Code hộ",
                            IndustryId = 1
                        },
                        new Category
                        {
                            Id = 2,
                            CategoryName = "Dọn dẹp",
                            IndustryId = 2
                        }
                    };

                await context.Categories.AddRangeAsync(categories);
            }

            if (!await context.UserProfiles.AnyAsync())
            {
                var userProfiles = new List<UserProfile>
                    {
                        new UserProfile
                        {
                            Id = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            CurrentSubscription = SubscriptionPlan.Premium,
                            SubscriptionExpiryDate = DateTime.UtcNow.AddYears(99),
                            Balance = 999999999,
                            Email = "huytde.dev@gmail.com",
                            FullName = "Huy Tran Duc",
                            Description = "Tôi là một lập trình viên, tôi yêu thích công việc của mình. Hãy để tôi giúp bạn!",
                            AvatarUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50",
                            CityId = "52",
                            WardId = "21550",
                            MajorIndustryId = 1,
                            MajorCategoryId = 1,
                            EducationHistory = ["FPT University Quy Nhon AI Campus"]
                        },

                        new UserProfile
                        {
                            Id = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            CurrentSubscription = SubscriptionPlan.Premium,
                            SubscriptionExpiryDate = DateTime.UtcNow.AddYears(99),
                            Balance = 999999999,
                            Email = "quynxqe170239@fpt.edu.vn",
                            FullName = "Quy Nguyen Xuan",
                            Description = "Tôi là một lập trình viên, tôi đã có nhiều năm kinh nghiệm làm việc trong lĩnh vực này.",
                            AvatarUrl = "https://gravatar.com/images/homepage/avatar-03.png",
                            CityId = "52",
                            WardId = "21550",
                            MajorIndustryId = 1,
                            MajorCategoryId = 1,
                            EducationHistory = ["FPT University Quy Nhon AI Campus"]
                        },

                        new UserProfile
                        {
                            Id = new Guid("3a3a3a3a-3a3a-3a3a-3a3a-3a3a3a3a3a3a"),
                            CurrentSubscription = SubscriptionPlan.Premium,
                            SubscriptionExpiryDate = DateTime.UtcNow.AddYears(99),
                            Balance = 999999999,
                            Email = "sangtnqe170193@fpt.edu.vn",
                            FullName = "Sang Tran Ngoc",
                            Description = "Tôi là một lập trình viên, tôi rất vui khi được giúp đỡ mọi người.",
                            AvatarUrl = "https://gravatar.com/images/homepage/avatar-02.png",
                            CityId = "52",
                            WardId = "21553",
                            MajorIndustryId = 1,
                            MajorCategoryId = 1,
                            EducationHistory = ["FPT University Quy Nhon AI Campus"]
                        },

                        new UserProfile
                        {
                            Id = new Guid("4a4a4a4a-4a4a-4a4a-4a4a-4a4a4a4a4a4a"),
                            CurrentSubscription = SubscriptionPlan.Premium,
                            SubscriptionExpiryDate = DateTime.UtcNow.AddYears(99),
                            Balance = 999999999,
                            Email = "huydt170135@fpt.edu.vn",
                            FullName = "Huy Dinh Trong",
                            Description = "Tôi là một lập trình viên, tôi rất vui khi được giúp đỡ mọi người.",
                            AvatarUrl = "https://gravatar.com/images/homepage/avatar-01.png",
                            CityId = "52",
                            WardId = "21571",
                            MajorIndustryId = 1,
                            MajorCategoryId = 1,
                            EducationHistory = ["FPT University Quy Nhon AI Campus"]
                        }
                    };

                await context.UserProfiles.AddRangeAsync(userProfiles);
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
                            Responsibilities = "Phát triển và duy trì ứng dụng web, đảm bảo hiệu suất cao.",
                            Fee = 1000000,
                            AssignedTo = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            DueDate = DateTime.UtcNow.AddDays(7),
                            CategoryId = 1,
                            IndustryId = 1,
                            CityId = "52",
                            WardId = "21550",
                            IsActive = false
                        },

                        new JobPost
                        {
                            Id = new Guid("2c2c2c2c-2c2c-2c2c-2c2c-2c2c2c2c2c2c"),
                            UserId = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            Title = "Code hộ",
                            Description = "Cần code hộ backend cho 1 ứng dụng di động",
                            Responsibilities = "Phát triển và duy trì ứng dụng di động, đảm bảo hiệu suất cao.",
                            Fee = 500000,
                            DueDate = DateTime.UtcNow.AddDays(30),
                            CategoryId = 1,
                            IndustryId = 1,
                            CityId = "52",
                            WardId = "21550",
                            IsActive = true
                        },

                        new JobPost
                        {
                            Id = new Guid("3c3c3c3c-3c3c-3c3c-3c3c-3c3c3c3c3c3c"),
                            UserId = new Guid("1a1a1a1a-1a1a-1a1a-1a1a-1a1a1a1a1a1a"),
                            Title = "Dọn dẹp nhà cửa",
                            Description = "Cần người dọn dẹp nhà cửa",
                            Responsibilities = "Dọn dẹp nhà cửa, lau chùi, quét dọn, vệ sinh nhà cửa.",
                            Fee = 300000,
                            AssignedTo = new Guid("2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a"),
                            DueDate = DateTime.UtcNow.AddDays(3),
                            CategoryId = 2,
                            IndustryId = 2,
                            CityId = "52",
                            WardId = "21556",
                            IsActive = false
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

                await context.SaveChangesAsync();
            }
            return app;
        }
    }
}