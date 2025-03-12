using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Subject.Deactivate;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Subject.Deactivate
{
    public class DeactivateSubjectUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var useCase = CreateUseCase(user, subject);

            Func<Task> act = async () => await useCase.Execute(subject.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Errror_Subject_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(subject.Id);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessages().Count() == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.SUBJECT_NOT_FOUND));
        }

        private static DeactivateSubjectUseCase CreateUseCase(
            StudyFlow.Domain.Entities.User user,
            StudyFlow.Domain.Entities.Subject? subject = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var updateOnlyRepository = new SubjectUpdateOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (subject is not null)
            {
                updateOnlyRepository.GetSubjectById(user, subject);
            }

            return new DeactivateSubjectUseCase(loggedUser, updateOnlyRepository.Build(), unitOfWork);
        }
    }
}
