using Moq;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Topic;

namespace CommonTestUtilities.Repositories
{
    public class TopicReadOnlyRepositoryBuilder
    {
        private readonly Mock<ITopicReadOnlyRepository> _mock;

        public TopicReadOnlyRepositoryBuilder()
        {
            _mock = new Mock<ITopicReadOnlyRepository>();
        }

        public TopicReadOnlyRepositoryBuilder IsTopicAlreadyCreated(User user, string? name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _mock.Setup(x => x.IsTopicAlreadyCreated(It.Is<string>(n => n.ToLower() == name.ToLower()), user, It.IsAny<long?>())).ReturnsAsync(true);
            }

            return this;
        }

        public TopicReadOnlyRepositoryBuilder GetAllTopics(User user, IList<Topic> topics)
        {
            _mock.Setup(x => x.GetAllTopics(user, null)).ReturnsAsync(topics);

            return this;
        }

        public TopicReadOnlyRepositoryBuilder GetTopicById(User user, Topic? topic)
        {
            _mock.Setup(x => x.GetTopicById(It.IsAny<long>(), user)).ReturnsAsync(topic);

            return this;
        }

        public ITopicReadOnlyRepository Build() => _mock.Object;
    }
}
