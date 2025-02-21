using Bogus;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int passwordLength = 10)
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(user => user.CurrentPassword, faker => faker.Internet.Password())
                .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(length: passwordLength));
        }
    }
}
