using Bogus;
using CommonTestUtilities.Security;
using StudyFlow.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user, string password) Build()
        {
            var encryption = new PasswordEncryptionBuilder().Build();

            var password = new Faker().Internet.Password();

            var user = new Faker<User>()
                .RuleFor(user => user.Id, _ => 1)
                .RuleFor(user => user.Name, faker => faker.Person.FirstName)
                .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
                .RuleFor(user => user.Password, _ =>  encryption.Encrypt(password));

           return (user, password);
        }
    }
}
