using StudyFlow.API.Converters;
using StudyFlow.API.Filters;
using StudyFlow.API.Middleware;
using StudyFlow.API.RateLimits;
using StudyFlow.API.Token;
using StudyFlow.Application;
using StudyFlow.Domain.Security.Token;
using StudyFlow.Infrastructure;
using StudyFlow.Infrastructure.DataAccess.Migrations;
using StudyFlow.Infrastructure.Extensions;
using StudyFlow.Infrastructure.Services.ReviewReminder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options => options.AddPolicy<string, RateLimiterPolicy>("RateLimiterPolicy"));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new StringConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudyFlow API", Version = "v1" });
});

builder.Services.AddMvc(option => option.Filters.Add<ExceptionFilters>());

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddHostedService<ReviewReminderBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyFlow API v1"));
    app.MapOpenApi();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!builder.Configuration.IsUnitTestEnviroment())
{
    app.UseRateLimiter();
}

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    if (builder.Configuration.IsUnitTestEnviroment())
    {
        return;
    }

    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program() { }
}
