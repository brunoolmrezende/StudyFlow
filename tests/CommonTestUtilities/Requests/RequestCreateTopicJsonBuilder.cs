using Bogus;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestCreateTopicJsonBuilder
    {
        public static RequestCreateTopicJson Build(string subjectId)
        {
            return new Faker<RequestCreateTopicJson>()
                .RuleFor(topic => topic.Name, faker => faker.Commerce.ProductName())
                .RuleFor(topic => topic.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(topic => topic.SubjectId, _ => subjectId);
        }
    }
}
