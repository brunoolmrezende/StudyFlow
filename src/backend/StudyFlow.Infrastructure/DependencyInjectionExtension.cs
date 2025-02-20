using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Domain.Security.Cryptography;
using StudyFlow.Domain.Security.Token;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Infrastructure.DataAccess;
using StudyFlow.Infrastructure.Extensions;
using StudyFlow.Infrastructure.Repositories;
using StudyFlow.Infrastructure.Security.Cryptography;
using StudyFlow.Infrastructure.Security.Token.Generate;
using StudyFlow.Infrastructure.Security.Token.Validate;
using StudyFlow.Infrastructure.Services.LoggedUser;
using System.Reflection;

namespace StudyFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddEncrypter(services);
            AddToken(services, configuration);
            AddLoggedUser(services);

            if (configuration.IsUnitTestEnviroment())
            {
                return;
            }

            AddDbContext(services, configuration);
            AddFluentMigrator_MySql(services, configuration);
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

            services.AddDbContext<StudyFlowDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseMySql(connectionString, serverVersion);
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddEncrypter(this IServiceCollection services)
        {
            services.AddScoped<IPasswordEncryption, PasswordEncryption>();
        }

        private static void AddToken(this IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<int>("Settings:Jwt:ExpirationTimeMinutes");
            var signInKey = configuration.GetValue<string>("Settings:Jwt:SignInKey");

            services.AddScoped<IAccessTokenGenerator>(options => new AccessTokenGenerator(expirationTimeMinutes, signInKey!));
            services.AddScoped<IAccessTokenValidator>(options => new AccessTokenValidator(signInKey!));
        }

        private static void AddLoggedUser(this IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
        }

        private static void AddFluentMigrator_MySql(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(opt =>
            {
                opt
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("StudyFlow.Infrastructure")).For.All();
            });
        }
    }
}
