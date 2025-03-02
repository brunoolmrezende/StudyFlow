using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Topic.Create;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Topic.Create
{
    public class CreateTopicUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var idEncoder = IdEncrypterBuilder.Build();

            var request = RequestCreateTopicJsonBuilder.Build(idEncoder.Encode(subject.Id));

            var useCase = CreateUseCase(user, subject);

            var result = await useCase.Execute(request);

            result.Name.Should().Be(request.Name);  
            result.Id.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_Topic_Already_Exists()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var idEncoder = IdEncrypterBuilder.Build();

            var request = RequestCreateTopicJsonBuilder.Build(idEncoder.Encode(subject.Id));

            var useCase = CreateUseCase(user, subject, request.Name);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.TOPIC_ALREADY_CREATED));
        }

        [Fact]
        public async Task Error_Subject_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var idEncoder = IdEncrypterBuilder.Build();

            var request = RequestCreateTopicJsonBuilder.Build(idEncoder.Encode(subject.Id));

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.SUBJECT_NOT_FOUND));
        }

        private static CreateTopicUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, StudyFlow.Domain.Entities.Subject? subject = null, string? topicName = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new TopicReadOnlyRepositoryBuilder();
            var writeOnlyRepository = TopicWriteOnlyRepositoryBuilder.Build();
            var subjectReadOnlyRepository = new SubjectReadOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var idEncoder = IdEncrypterBuilder.Build();

            if (subject is not null)
            {
                subjectReadOnlyRepository.GetSubjectById(user, subject);
            }

            if (!string.IsNullOrWhiteSpace(topicName))
            {
                readOnlyRepository.IsTopicCreatedAndActive(user, topicName);
            }

            return new CreateTopicUseCase(
                loggedUser, 
                readOnlyRepository.Build(), 
                writeOnlyRepository, 
                subjectReadOnlyRepository.Build(), 
                unitOfWork, 
                mapper, 
                idEncoder);
        }
    }
}
