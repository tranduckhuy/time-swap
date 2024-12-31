using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TimeSwap.Application;
using TimeSwap.Infrastructure.Extensions;
using TimeSwap.Infrastructure.Persistence.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase<AppDbContext>(builder.Configuration);
builder.Services.AddHealthChecks().Services.AddDbContext<AppDbContext>();
builder.Services.AddCoreInfrastructure(builder.Configuration);

builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Load location data
await app.LoadLocationDataAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.SeedCoreDataAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();
