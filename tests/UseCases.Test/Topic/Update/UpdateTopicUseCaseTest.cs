using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Topic.Update;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;
using System.Reflection;

namespace UseCases.Test.Topic.Update
{
    public class UpdateTopicUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(subject.Id));

            var useCase = CreateUseCase(user, topic, subject);

            Func<Task> act = async () => await useCase.Execute(topic.Id, request);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(subject.Id));

            var useCase = CreateUseCase(user: user, subject: subject);

            Func<Task> act = async () => await useCase.Execute(1, request);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Topic_Already_Exists()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(subject.Id));

            var useCase = CreateUseCase(user: user, subject: subject, topicName: request.Name);

            Func<Task> act = async () => await useCase.Execute(topic.Id, request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.TOPIC_ALREADY_CREATED));
        }

        [Fact]
        public async Task Error_Subject_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var topic = TopicBuilder.Build(user, subject.Id);

            var request = RequestUpdateTopicJsonBuilder.Build(IdEncrypterBuilder.Build().Encode(subject.Id));

            var useCase = CreateUseCase(user: user, topic: topic);

            Func<Task> act = async () => await useCase.Execute(topic.Id, request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.SUBJECT_NOT_FOUND));
        }

        private static UpdateTopicUseCase CreateUseCase(
            StudyFlow.Domain.Entities.User user, 
            StudyFlow.Domain.Entities.Topic? topic = null,
            StudyFlow.Domain.Entities.Subject? subject = null,
            string? topicName = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new TopicReadOnlyRepositoryBuilder();
            var updateOnlyRepository = new TopicUpdateOnlyRepositoryBuilder();
            var subjectReadOnlyRepository = new SubjectReadOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var idEncoder = IdEncrypterBuilder.Build();

            if (!string.IsNullOrWhiteSpace(topicName))
            {
                readOnlyRepository.IsTopicAlreadyCreated(user, topicName);
            }

            if (subject is not null)
            {
                subjectReadOnlyRepository.GetSubjectById(user, subject);
            }

            if (topic is not null)
            {
                updateOnlyRepository.GetTopicById(user, topic);
            }

            return new UpdateTopicUseCase(
                loggedUser,
                readOnlyRepository.Build(),
                subjectReadOnlyRepository.Build(),
                updateOnlyRepository.Build(),
                unitOfWork,
                mapper,
                idEncoder
            );
        }
    }
}
