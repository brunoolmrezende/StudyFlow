using Bogus;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestCreateSubjectJsonBuilder
    {
        public static RequestCreateSubjectJson Build()
        {
            return new Faker<RequestCreateSubjectJson>()
                .RuleFor(subject => subject.Name, faker => faker.Commerce.ProductName());
        }
    }
}
