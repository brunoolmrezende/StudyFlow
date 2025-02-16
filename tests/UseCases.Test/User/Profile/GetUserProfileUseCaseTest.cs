using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using FluentAssertions;
using StudyFlow.Application.UseCases.User.GetProfile;

namespace UseCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Email.Should().Be(user.Email);
            result.Name.Should().Be(user.Name);
        }

        private static GetUserProfileUseCase CreateUseCase(StudyFlow.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();

            return new GetUserProfileUseCase(loggedUser, mapper);
        }
    }
}
