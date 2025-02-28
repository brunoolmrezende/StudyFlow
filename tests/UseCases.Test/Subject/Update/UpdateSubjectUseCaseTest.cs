using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Subject.Update;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Subject.Update
{
    public class UpdateSubjectUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var request = RequestUpdateSubjectJsonBuilder.Build();

            var useCase = CreateUseCase(user, subject);

            Func<Task> act = async () => await useCase.Execute(subject.Id, request);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var request = RequestUpdateSubjectJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(subject.Id, request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public async Task Error_Subject_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var request = RequestUpdateSubjectJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(subject.Id, request);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.SUBJECT_NOT_FOUND));
        }

        private static UpdateSubjectUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, StudyFlow.Domain.Entities.Subject? subject = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateOnlyRepository = new SubjectUpdateOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (subject is not null)
            {
                updateOnlyRepository.GetSubjectById(user, subject);
            }

            return new UpdateSubjectUseCase(loggedUser, updateOnlyRepository.Build(), unitOfWork);
        }
    }
}
