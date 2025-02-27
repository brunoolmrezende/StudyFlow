using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Subject.GetById;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Subject.GetById
{
    public class GetSubjectByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var useCase = CreateUseCase(user, subject);

            var result = await useCase.Execute(subject.Id);

            result.Should().NotBeNull();
            result.Id.Should().NotBeNullOrWhiteSpace();
            result.Name.Should().Be(subject.Name);
        }

        [Fact]
        public async Task Error_Subject_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var subject = SubjectBuilder.Build(user);

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(subject.Id);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.SUBJECT_NOT_FOUND));
        }

        private static GetSubjectByIdUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, StudyFlow.Domain.Entities.Subject? subject = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var readOnlyRepository = new SubjectReadOnlyRepositoryBuilder();

            if (subject is not null)
            {
                readOnlyRepository.GetSubjectById(user, subject);
            }

            return new GetSubjectByIdUseCase(loggedUser, readOnlyRepository.Build(), mapper);
        }
    }
}
