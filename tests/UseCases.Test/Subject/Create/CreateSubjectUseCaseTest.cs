using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using StudyFlow.Application.UseCases.Subject.Create;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Subject.Create
{
    public class CreateSubjectUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestCreateSubjectJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Id.Should().NotBeNullOrWhiteSpace();
            result.Name.Should().Be(request.Name);
        }

        [Fact]
        public async Task Error_Empty_Name()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestCreateSubjectJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
               .Where(error => error.GetErrorMessages().Count == 1
                   && error.GetErrorMessages().Contains(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public async Task Error_Subject_Already_Created()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestCreateSubjectJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Name);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.SUBJECT_ALREADY_CREATED));
        }

        private static CreateSubjectUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, string? subjectName = null)
        {
            var writeOnlyRepository = SubjectWriteOnlyRepositoryBuilder.Build();
            var readOnlyRepository = new SubjectReadOnlyRepositoryBuilder();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();

            if (subjectName is not null)
            {
                readOnlyRepository.IsSubjectAlreadyCreated(user, subjectName);
            }

            return new CreateSubjectUseCase(writeOnlyRepository, readOnlyRepository.Build(), unitOfWork, loggedUser, mapper);
        }
    }
}
