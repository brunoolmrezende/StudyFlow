using Moq;
using StudyFlow.Domain.Repositories.Topic;

namespace CommonTestUtilities.Repositories
{
    public class TopicUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<ITopicUpdateOnlyRepository> _mock;

        public TopicUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<ITopicUpdateOnlyRepository>();
        }

        public TopicUpdateOnlyRepositoryBuilder GetTopicById(
            StudyFlow.Domain.Entities.User loggedUser, 
            StudyFlow.Domain.Entities.Topic? topic = null)
        {
            if (topic is not null)
            {
                _mock.Setup(repository => repository.GetTopicById(topic.Id, loggedUser)).ReturnsAsync(topic);
            }

            return this;
        }

        public ITopicUpdateOnlyRepository Build() => _mock.Object;
    }
}
