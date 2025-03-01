using Moq;
using StudyFlow.Domain.Repositories.Topic;

namespace CommonTestUtilities.Repositories
{
    public class TopicWriteOnlyRepositoryBuilder
    {
        public static ITopicWriteOnlyRepository Build()
        {
            var mock = new Mock<ITopicWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
