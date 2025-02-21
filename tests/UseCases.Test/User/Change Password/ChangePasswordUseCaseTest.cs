using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Application.UseCases.User.ChangePassword;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace UseCases.Test.User.Change_Password
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.CurrentPassword = password;

            var useCase = CreateUseCase(user, request.CurrentPassword);

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.Should().NotThrowAsync();

            var passwordEncryption = new PasswordEncryptionBuilder().Build();

            user.Password.Should().Be(passwordEncryption.Encrypt(request.NewPassword));
        }

        [Fact]
        public async Task Error_Password_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;

            var useCase = CreateUseCase(user, request.CurrentPassword);

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1 &&
                    error.GetErrorMessages().Contains(ResourceMessagesException.EMPTY_PASSWORD));
        }

        [Fact]
        public async Task Error_Current_Password_Invalid()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessages().Count == 1 &&
                    error.GetErrorMessages().Contains(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        private static ChangePasswordUseCase CreateUseCase(StudyFlow.Domain.Entities.User user, string? password = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var encryption = new PasswordEncryptionBuilder().Verify(password).Build();
            var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ChangePasswordUseCase(loggedUser, encryption, updateOnlyRepository, unitOfWork);
        }
    }
}
