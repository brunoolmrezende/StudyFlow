using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using StudyFlow.Application.UseCases.Subject.GetAll;

namespace UseCases.Test.Subject.GetAll
{
    public class GetAllSubjectsUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var subjects = SubjectBuilder.Collection(user);

            var useCase = CreateUseCase(user, subjects);

            var result = await useCase.Execute(null);

            result.Should().NotBeNull();
            result.Subjects.Should()
                .HaveCountGreaterThan(0)
                .And.OnlyHaveUniqueItems(subject => subject.Id)
                .And.AllSatisfy(subject =>
                {
                    subject.Id.Should().NotBeNullOrWhiteSpace();
                    subject.Name.Should().NotBeNullOrWhiteSpace();
                });
        }

        private static GetAllSubjectsUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, IList<StudyFlow.Domain.Entities.Subject> subjects)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var readOnlyRepository = new SubjectReadOnlyRepositoryBuilder().GetAllSubjects(user, subjects).Build();

            return new GetAllSubjectsUseCase(loggedUser, readOnlyRepository, mapper);
        }
    }
}
