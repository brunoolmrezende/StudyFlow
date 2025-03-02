using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Topic.GetAll;

namespace UseCases.Test.Topic.GetAll
{
    public class GetAllTopicsUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var topics = TopicBuilder.Collection(user, 1);

            var useCase = CreateUseCase(user, topics);

            var result = await useCase.Execute(null);

            result.Should().NotBeNull();
            result.Topics.Should()
                .HaveCountGreaterThan(0)
                .And.OnlyHaveUniqueItems(topic => topic.Id)
                .And.AllSatisfy(topic =>
                {
                    topic.Id.Should().NotBeNullOrWhiteSpace();
                    topic.Name.Should().NotBeNullOrWhiteSpace();
                });
        }

        private static GetAllTopicsUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, IList<StudyFlow.Domain.Entities.Topic> topics)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var readOnlyRepository = new TopicReadOnlyRepositoryBuilder().GetAllTopics(user, topics).Build();

            return new GetAllTopicsUseCase(loggedUser, readOnlyRepository, mapper);
        }
    }
}
