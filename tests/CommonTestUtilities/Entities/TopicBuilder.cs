using Bogus;
using StudyFlow.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class TopicBuilder
    {
        public static IList<Topic> Collection(User user, long subjectId, uint count = 2)
        {
            var list = new List<Topic>();

            if (count == 0)
            {
                count = 1;
            }

            var topicId = 1;

            for (int i = 0; i < count; i++)
            {
                var fakeTopic = Build(user, subjectId);
                fakeTopic.Id = topicId++;

                list.Add(fakeTopic);
            }

            return list;
        }

        public static Topic Build(User user, long subjectId)
        {
            return new Faker<Topic>()
                .RuleFor(topic => topic.Id, _ => 1)
                .RuleFor(topic => topic.Name, faker => faker.Commerce.ProductName())
                .RuleFor(topic => topic.Description, faker => faker.Lorem.Sentence())
                .RuleFor(topic => topic.SubjectId, _ => subjectId)
                .RuleFor(topic => topic.UserId, _ => user.Id);
        }
    }
}
