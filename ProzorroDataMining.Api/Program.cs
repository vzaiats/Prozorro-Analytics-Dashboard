using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProzorroDataMining.Api.Configurations;
using ProzorroDataMining.Api.Extensions;
using ProzorroDataMining.Api.Mapping;
using ProzorroDataMining.Api.Middleware;
using ProzorroDataMining.Application.Interfaces.Services;
using ProzorroDataMining.Application.Services;
using ProzorroDataMining.Data.Database;
using ProzorroDataMining.Data.Repository;
using ProzorroDataMining.Domain.Interfaces.Repositories;
using Serilog;
using Serilog.Events;
using TaskListsAPI.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Logging
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

// EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add DbConnection for Dapper
builder.Services.AddScoped<System.Data.Common.DbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<ITenderRepository, TenderRepository>();

// Add services
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<ITenderIngestionService, TenderIngestionService>();

// Add AutoMapper registration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AnalyticsMappingProfile>();
});

// Add AddProzorroHttpClient for Prozorro API
builder.Services.AddProzorroHttpClient();

// Add API configurations
builder.Services.Configure<ProzorroApiOptions>(
    builder.Configuration.GetSection("ProzorroApi"));

// Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100; // Max requests per window
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 10;
        limiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

// Swagger
builder.Services.AddSwaggerDocumentation();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// HealthChecks
builder.Services.AddAppHealthChecks(builder.Configuration);

var app = builder.Build();

// Recreate database on each start (Development) or apply migrations (Production)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (app.Environment.IsDevelopment())
    {
        // Development
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
    else
    {
        // Production
        db.Database.Migrate();
    }
}

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseRateLimiter();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Map health checks
app.MapAppHealthChecks();

// Routing
app.MapControllers();

// Run
app.Run();
