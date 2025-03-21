using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TimeSwap.Application;
using TimeSwap.Auth.Mappings;
using TimeSwap.Infrastructure.Extensions;
using TimeSwap.Infrastructure.Identity;
using TimeSwap.Infrastructure.Middlewares;
using TimeSwap.Infrastructure.Persistence.DbContexts;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// SuppressModelStateInvalidFilter can be used to disable the automatic 400 response for invalid models.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddApplicationIdentity<ApplicationUser>();
builder.Services.AddApplicationJwtAuth(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    opt.TokenLifespan = TimeSpan.FromMinutes(5);
});

builder.Services.AddDatabase<AppDbContext>(builder.Configuration.GetConnectionString("CoreDbConnection")
                ?? throw new InvalidDataException("The CoreDbConnection string is missing in the configuration."));

builder.Services.AddDatabase<UserIdentityDbContext>(builder.Configuration.GetConnectionString("AuthDbConnection")
                ?? throw new InvalidDataException("The AuthDbConnection string is missing in the configuration."));

builder.Services.AddHealthChecks().Services.AddDbContext<UserIdentityDbContext>();
builder.Services.AddAutoMapper(typeof(AuthMappingProfile));
builder.Services.AddAuthInfrastructure(builder.Configuration);
builder.Services.AddModelValidator();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedAuthDataAsync();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<TokenValidationMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();
