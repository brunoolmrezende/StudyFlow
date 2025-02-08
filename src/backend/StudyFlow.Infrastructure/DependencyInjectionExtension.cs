using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Domain.Security.Cryptography;
using StudyFlow.Infrastructure.DataAccess;
using StudyFlow.Infrastructure.Repositories;
using StudyFlow.Infrastructure.Security;

namespace StudyFlow.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddEncrypter(services);
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionMySQLServer");

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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddEncrypter(this IServiceCollection services)
        {
            services.AddScoped<IPasswordEncryption, PasswordEncryption>();
        }
    }
}
