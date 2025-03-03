using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Topic.GetById;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Topic.GetById
{
    public class GetTopicByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user, 1);

            var useCase = CreateUseCase(user, topic);

            var result = await useCase.Execute(topic.Id);

            result.Should().NotBeNull();
            result.Id.Should().NotBeNullOrWhiteSpace();
            result.SubjectId.Should().NotBeNullOrWhiteSpace();
            result.Name.Should().Be(topic.Name);
            result.Description.Should().Be(topic.Description);
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(1);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        private static GetTopicByIdUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, StudyFlow.Domain.Entities.Topic? topic = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new TopicReadOnlyRepositoryBuilder();
            var mapper = MapperBuilder.Build();

            if (topic is not null)
            {
                readOnlyRepository.GetTopicById(user, topic);
            }

            return new GetTopicByIdUseCase(loggedUser, readOnlyRepository.Build(), mapper);
        }
    }
}
