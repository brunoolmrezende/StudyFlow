using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyFlow.Domain.Security.Cryptography;
using StudyFlow.Infrastructure.DataAccess;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private StudyFlow.Domain.Entities.User _user = default!;
        private StudyFlow.Domain.Entities.Subject _subject = default!;
        private string _password = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<StudyFlowDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<StudyFlowDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    using var scope = services.BuildServiceProvider().CreateScope();

                    var database = scope.ServiceProvider.GetRequiredService<StudyFlowDbContext>();

                    database.Database.EnsureDeleted();

                    var encrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncryption>();

                    StartDatabase(database, encrypter);
                });
    
        }

        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
        public string GetUserName() => _user.Name;
        public Guid GetUserIdentifier() => _user.UserIdentifier;

        private void StartDatabase(StudyFlowDbContext dbContext, IPasswordEncryption encrypter)
        {
            (_user, _password) = UserBuilder.Build();

            _subject = SubjectBuilder.Build(_user);

            _user.Password = encrypter.Encrypt(_password);

            dbContext.Database.EnsureCreated();

            dbContext.Users.Add(_user);
            dbContext.Subjects.Add(_subject);

            dbContext.SaveChanges();
        }
    }
}
