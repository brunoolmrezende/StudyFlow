using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Topic.Deactivate;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Topic.Deactivate
{
    public class DeactivateTopicUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var useCase = CreateUseCase(user, topic);

            Func<Task> act = async () => await useCase.Execute(topic.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Errror_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(topic.Id);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessages().Count() == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        private static DeactivateTopicUseCase CreateUseCase(
            StudyFlow.Domain.Entities.User user,
            StudyFlow.Domain.Entities.Topic? topic = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateOnlyRepository = new TopicUpdateOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (topic is not null)
            {
                updateOnlyRepository.GetTopicById(user, topic);
            }

            return new DeactivateTopicUseCase(loggedUser, updateOnlyRepository.Build(), unitOfWork);
        }
    }
}
