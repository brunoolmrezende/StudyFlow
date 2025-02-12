using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Application.UseCases.User.Login.DoLogin;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var useCase = CreateUseCase(user, password);

            var request = new RequestDoLoginJson
            {
                Email = user.Email,
                Password = password,
            };

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
        }

        [Fact]
        public async Task Error_Invalid_Email_Or_Password()
        {
            var request = RequestDoLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<InvalidLoginException>()
                .Where(error => error.GetErrorMessages().Count() == 1
                    && error.GetErrorMessages().Contains(ResourceMessagesException.INVALID_EMAIL_OR_PASSWORD));
        }

        private static DoLoginUseCase CreateUseCase(StudyFlow.Domain.Entities.User ?user = null, string? password = null)
        {
            var encryption = new PasswordEncryptionBuilder().Verify(password).Build();
            var repository = new UserReadOnlyRepositoryBuilder();

            if (user is not null)
            {
                repository.GetUserByEmail(user);
            }

            return new DoLoginUseCase(repository.Build(), encryption);
        }
    }
}
