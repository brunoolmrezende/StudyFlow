using Bogus;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateSubjectJsonBuilder
    {
        public static RequestUpdateSubjectJson Build()
        {
            return new Faker<RequestUpdateSubjectJson>()
                .RuleFor(subject => subject.Name, faker => faker.Commerce.ProductName())
                .RuleFor(subject => subject.Active, faker => faker.Random.Bool());
        }
    }
}
