using Bogus;
using CommonTestUtilities.IdEncrypter;
using StudyFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateTopicJsonBuilder
    {
        public static RequestUpdateTopicJson Build(string subjectId)
        {
            return new Faker<RequestUpdateTopicJson>()
                .RuleFor(topic => topic.Name, faker => faker.Lorem.Word())
                .RuleFor(topic => topic.Description, faker => faker.Lorem.Sentence())
                .RuleFor(topic => topic.SubjectId, _ => subjectId)
                .RuleFor(topic => topic.Active, faker => faker.Random.Bool());
        }
    }
}
