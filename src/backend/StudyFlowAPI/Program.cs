using StudyFlow.API.Converters;
using StudyFlow.API.Filters;
using StudyFlow.API.Middleware;
using StudyFlow.Application;
using StudyFlow.Infrastructure;
using StudyFlow.Infrastructure.DataAccess.Migrations;
using StudyFlow.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new StringConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMvc(option => option.Filters.Add<ExceptionFilters>());

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

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
